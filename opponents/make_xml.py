# -*- coding: utf-8 -*-

import sys
import os
import imp
if sys.version_info[0] == 2:
	imp.reload(sys)
	sys.setdefaultencoding('utf8')
import xml.etree.ElementTree as ET
import xml.dom.minidom as minidom
import datetime
import re
from collections import OrderedDict
from ordered_xml import OrderedXMLElement as Element, Comment
try:
     # Python 2.6-2.7
     from HTMLParser import HTMLParser
except ImportError:
     # Python 3
     from html.parser import HTMLParser

unescapeHTML = HTMLParser().unescape

#tags that relate to ending sequences
ending_tag = "ending" #name for the ending
ending_gender_tag = "ending_gender" #player gender the ending is shown to
ending_preview_tag = "gallery_image" # image to use for the preview in the gallery
screen_tag = "screen"
text_tag = "text"
x_tag = "x"
y_tag = "y"
width_tag = "width"
arrow_tag = "arrow"
ending_tags = [ending_tag, ending_gender_tag, ending_preview_tag, screen_tag, text_tag, x_tag, y_tag, width_tag, arrow_tag]
situations = []

#sets of possible targets for lines
one_word_targets = ["target", "filter"]
multi_word_targets = ["targetStage", "targetLayers", "targetStatus", "alsoPlaying", "alsoPlayingStage", "alsoPlayingHand", "oppHand", "hasHand", "totalMales", "totalFemales", "targetTimeInStage", "alsoPlayingTimeInStage", "timeInStage", "consecutiveLosses", "totalAlive", "totalExposed", "totalNaked", "totalMasturbating", "totalFinished", "totalRounds", "saidMarker", "notSaidMarker", "alsoPlayingSaidMarker", "alsoPlayingNotSaidMarker", "targetSaidMarker", "targetNotSaidMarker", "priority"] #these will need to be re-capitalised when writing the xml
lower_multi_targets = [t.lower() for t in multi_word_targets]
all_targets = one_word_targets + lower_multi_targets

def capitalizeDialogue(s):
	# Convert the first character of the string, or of a variable that starts the string, to uppercase
	return re.sub('(?<=^~)[a-z](?=\w+~)|^\w', lambda m: m.group(0).upper(), s)

def typographicalize(s):
	substitutions = [u'\N{LEFT DOUBLE QUOTATION MARK}',
			 u'\N{LEFT SINGLE QUOTATION MARK}',
			 u'\N{RIGHT DOUBLE QUOTATION MARK}',
			 u'\N{RIGHT SINGLE QUOTATION MARK}',
			 u'\N{HORIZONTAL ELLIPSIS}']
	return re.sub("(``)|(`)|('')|(')|(\.\.\.)",
		      # Find the substitution corresponding to the matching group
		      lambda m: next(iter([r for g, r in zip(m.groups(), substitutions) if g])),
		      # First pass to substitute pairs of double quotes
		      re.sub('"([^"]*)"', u'\N{LEFT DOUBLE QUOTATION MARK}\\1\N{RIGHT DOUBLE QUOTATION MARK}', s))

def fixupDialogue(s):
	return capitalizeDialogue(typographicalize(s))

def get_situations_from_xml():
	filename = os.path.join(os.path.dirname(sys.argv[0]), 'dialogue_tags.xml')
	dialogue_tags = ET.parse(filename)
	for el in dialogue_tags.iterfind('./triggers/trigger'):
		if el.attrib['tag'] == '-':
			continue
		situations.append({
			'key': el.get('tag'),
			'start': int(el.get('start', '0')),
			'end': int(el.get('end', '10')),
			'group': int(el.get('group', '0')),
			'order': int(el.get('order', '0')),
			'image': el.findtext('defaultImage'),
			'text': el.findtext('defaultText'),
			'optional': el.get('optional') == 'true',
		})
	situations.sort(key=lambda x: (x['group'], x['order']))

#get a set of cases from the dictionaries. First try stage-specific from the character's data, then general entries from the character's data, then stage-specific from the default data, then general cases from the default data.
def get_cases(player_dictionary, situation, stage):
	image_formats = ["png", "jpg", "jpeg", "gif", "gifv"] #image file format extensions
	out_list = []
	key = situation['key']
	full_key = "%d-%s" % (stage, key)

	result_list = list()
	def is_generic_line(line_data):
		for target_type in all_targets:
			if target_type in line_data:
				return False
		return True

	def have_generic_line(lines):
		for line_data in lines:
			if is_generic_line(line_data):
				return True
		return False
	
	have_generic_entry = False

	#check character's data
	if full_key in player_dictionary:
		result_list += player_dictionary[full_key]
		
		#check if whe have a line that doesn't have any targets or filters
		#because we need at least one line that doesn't have one
		if have_generic_line(result_list):
			have_generic_entry = True
		
	if key in player_dictionary:
		for line_data in player_dictionary[key]:
			# Don't add completely generic lines to a given stage when a
			# stage-specific generic case exist for that stage,
			# but do add targeted lines (because it's too complicated to
			# look for matching targeted cases and it shouldn't cause any
			# conflicts with workarounds for incorrectly added defaults).
			if not is_generic_line(line_data) or not have_generic_entry:
				result_list.append(line_data)

		if have_generic_line(result_list):
			have_generic_entry = True
	
	#use the default data if there are no player-specific lines available
	if not have_generic_entry and not situation['optional']:
		result_list.append({'key': situation['key'], 'text': situation['text'], 'image': situation['image']})
		print("Warning: Using default line for key %s, stage %d" % (key, stage))
	
	#debug
	#convert image formats
	#print "result list", result_list #for debug purposes
	for i, line_data in enumerate(result_list):
		line_data = line_data.copy() #use a copy of the line_data entry
		#because if we copy it then changing the stage number for images (below) for lines that don't have stage numbers
		#will use the first stage number that doesn't have a stage-specific version for all the stages where the generic line is used
	
		image = line_data["image"]
		text = line_data["text"]
		if len(image) <= 0:
			#if the character entry doesn't include an image, use default image
			image = situation["image"]
		
		#if the image name doesn't include a stage, prepend the current stage
		if not image[0].isdigit():
			image = "%d-%s" % (stage, image)
		
		#if no file extension, assume .png
		if "." not in image:
			image += ".png"
		else:
			name, extention = image.rsplit(".", 1)
			if extention not in image_formats:
				#if the image name doesn't end with a known image format, assume it's a .png file that just happens to have a . in its name
				image += ".png"
		
		line_data["image"] = image
		
		#out_list.append( (image+".png", text) ) don't use this
		out_list.append( line_data ) #because we switched to using dictionaries
	
	return out_list

#add a single emenent (initially used so I can add a tag named "tag")
#now it also handles targets, which are optional
#now it takes a series of lines for a particular stage, and adds all the <case> and <state> elements for the given list of lines
def create_case_xml(base_element, lines):
	#one_word_targets = ["target", "filter"]
	#targets = one_word_targets + ["targetstage"]
	
	#step 1: sort the lines by case (situation, and any targets)
	#this means that once the case changes, we know that the script won't see that case again
	#give them a key to define an order
	for line_data in lines:
		sort_key = line_data["key"]
		if "conditions" in line_data:
			for condition in line_data["conditions"]:
				sort_key += "," + "count-" + condition[0]
		if "tests" in line_data:
			for test in line_data["tests"]:
				sort_key += "," + "test:" + test[0]
		for target_type in all_targets:
			if target_type in line_data:
				sort_key += "," + target_type + ":" +line_data[target_type]
		line_data["sort_key"] = sort_key

	#now do the sorting
	lines.sort(key=lambda l: l["sort_key"])
	
	#step 2: iterate through the list of lines
	current_sort = "" #which case combination we're currently looking at. initially nothing
	case_xml_element = None #current XML element, add states to this

	possible_statuses = [ 'alive', 'lost_some', 'mostly_clothed', 'decent', 'exposed',
			      'chest_visible', 'crotch_visible', 'topless', 'bottomless',
			      'naked', 'lost_all', 'masturbating', 'finished' ]
	
	for line_data in lines:
		if line_data["sort_key"] != current_sort:
			#this is a new key
			current_sort = line_data["sort_key"]
			
			#make a new <case> element in the xml
			tag_list = OrderedDict(tag=line_data["key"]) #every case needs a "tag" value that denotes the situation
			
			for target_type in one_word_targets:
				if target_type in line_data:
					tag_list[target_type] = line_data[target_type]
			
			#need to re-capitalise multi-word target names
			for ind, lower_case_target in enumerate(lower_multi_targets):
				if lower_case_target in line_data:
					capital_word = multi_word_targets[ind]
					tag_list[capital_word] = line_data[lower_case_target]
	
			case_xml_element = base_element.subElement("case", None, tag_list) #create the <case> element in the xml

			if "conditions" in line_data:
				for condition in line_data["conditions"]:
					conddict = OrderedDict(count=condition[1])
					condparts = condition[0].split('&') if condition[0] != '' else []
					for cond in condparts:
						if cond in [ 'male', 'female' ]:
							conddict['gender'] = cond
						elif cond in possible_statuses or (cond[0:4] == 'not_' and cond[4:] in possible_statuses):
							conddict['status'] = cond
						else:
							conddict['filter'] = cond

					case_xml_element.subElement("condition", None, conddict)

			if "tests" in line_data:
				for test in line_data["tests"]:
					case_xml_element.subElement("test", [('expr', test[0]), ('value', test[1])])


		#now add the individual line
		#remember that this happens regardless of if the <case> is new
		attrib = OrderedDict(img=line_data["image"])
		if "marker" in line_data:
			attrib["marker"] = line_data["marker"]
		if "direction" in line_data:
			attrib["direction"] = line_data["direction"]
		if "location" in line_data:
			attrib["location"] = line_data["location"]
		case_xml_element.subElement("state", line_data["text"], attrib) #add the image and text

#add several values to the XML tree
#specifically, adds the <case> and <state> elements to a <stage> base_element
def add_values(base_element, player_dictionary, stage):
	clothes_count = len(player_dictionary["clothes"])
	def adjust_stage(stage):
		if stage > 4:
			return stage - 8 + clothes_count
		else:
			return stage

	for situation in situations:
		if stage < adjust_stage(situation['start']) or stage > adjust_stage(situation['end']):
			continue
		contents = get_cases(player_dictionary, situation, stage)
		#add the target values, if any
		create_case_xml(base_element, contents) #add the case element to the XML tree

#write the xml file to the specified filename
def write_xml(data, filename):
	#f = open(filename)
	o = Element("opponent")
	mydate = datetime.datetime.now()
	o.append(Comment("This file was machine generated by make_xml.py version 2.0 in " + mydate.strftime("%B") + " " + mydate.strftime("%Y") +". Please do not edit it directly without preserving your improvements elsewhere or your changes may be lost the next time this file is generated."))
	o.subElement("first", data["first"])
	o.subElement("last", data["last"])

	#label
	for stage in data["label"]:
		if stage == 0:
			o.subElement("label", data["label"][stage])
		else:
			o.subElement("label", data["label"][stage], {stage: stage})

	for tag in ("gender", "size", "timer"):
		o.subElement(tag, data[tag])

	#intelligence
	for stage in data["intelligence"]:
		if stage == 0:
			o.subElement("intelligence", data["intelligence"][stage])
		else:
			o.subElement("intelligence", data["intelligence"][stage], {'stage': stage})

	#tags
	tags_elem = o.subElement("tags")
	character_tags = data["character_tags"]
	for tag in character_tags:
		tags_elem.subElement("tag", tag)

	#start image
	start = o.subElement("start")
	start_data = data["start"] if "start" in data else ["0-calm,So we'll be playing strip poker... I hope we have fun."]
	start_count = len(start_data)
	for i in range(0, start_count):
		start_image, start_text = start_data[i].split(",", 1)
		start.subElement("state", start_text, {'img': start_image+".png"})
	
	#wardrobe
	clth = o.subElement("wardrobe")
	clothes = data["clothes"]
	clothes_count = len(clothes)
	for i in range(clothes_count - 1, -1, -1):
		pname, lname, tp, pos, num = (clothes[i] + ",").split(",")[:5]
		clothesattr = OrderedDict([("lowercase", lname), ("position", pos), ("proper-name", pname), ("type", tp)])
		if num=="plural":
			clothesattr["plural"] = "true"
		clth.subElement("clothing", None, clothesattr)
	
	#behaviour
	bh = o.subElement("behaviour")
	for stage in range(0, clothes_count+3):
		s = bh.subElement("stage", None, {'id': str(stage)})
		add_values(s, data, stage)
	
	#endings
	if "endings" in data:
		#for each ending
		for ending in data["endings"]:
			ending_xml = o.subElement("epilogue", None, {'gender': ending["gender"]})
			
			if 'img' in ending:
				ending_xml.set('img', ending['img'])
			
			ending_xml.subElement("title", ending["title"])
			
			#for each screen in an ending
			for screen in ending["screens"]:
				screen_xml = ending_xml.subElement("screen", None, {'img': screen["image"]})
				
				#for each text box on a screen
				for text_box in screen["text_boxes"]:
					text_box_xml = screen_xml.subElement("text")
					text_box_xml.subElement(x_tag, text_box[x_tag])
					text_box_xml.subElement(y_tag, text_box[y_tag])
					#width and arrow are optional
					if width_tag in text_box:
						text_box_xml.subElement(width_tag, text_box[width_tag])
					if arrow_tag in text_box:
						text_box_xml.subElement(arrow_tag, text_box[arrow_tag])
					text_box_xml.subElement("content", fixupDialogue(text_box[text_tag]))
	
	#done
	
	open(filename, 'w').write(o.serialize())


#add an ending to the 
def add_ending(ending, d):
	ending = dict(ending)

	if len(list(ending.keys())) <= 0:
		#this is an empty ending, so don't add anything
		return
	
	#check for required values
	if "title" not in ending:
		print("Error - ending \"%s\" does not have a title." % (str(ending)))
		return
		
	if "gender" not in ending:
		print("Error - ending \"%s\" does not have a gender specified." % (str(ending)))
		return
		
	if "screens" not in ending:
		print("Error - ending \"%s\" does not have any screens." % (str(ending)))
		return
	
	#either get the endings data from the dictionary, or make a new endings variable and add that to the dictionary
	endings = None
	if "endings" in d:
		endings = d["endings"]
	else:
		endings = list()
		d["endings"] = endings
		
	endings.append(ending)
	
#handle the ending data
def handle_ending_string(key, content, ending, d):
	if key == ending_tag:
		#this is a new ending, so store the previous ending (if any)
		add_ending(ending, d)
		#reset the ending data
		ending.clear()
		#and add the title of the new ending
		ending["title"] = content
		return
	elif key == ending_gender_tag:
		if len(content) <= 0: #if the gender wasn't specified, use "any"
			content = "any"
		ending["gender"] = content
		return
	elif key == ending_preview_tag:
		if len(content) > 0:
			ending['img'] = content
		return
		
	#get the screens variable
	screens = None
	if "screens" in ending:
		screens = ending["screens"]
	else:
		#or make one, if it doesn't already exist
		screens = list()
		ending["screens"] = screens
		
	#get the current screen
	screen = None
	if len(screens) >= 1:
		screen = screens[-1]
	
	#background image for a screen - makes a new screen
	if key == screen_tag:
		screen = dict()
		screens.append(screen)
		screen["image"] = content
		screen["text_boxes"] = list()
		return
	
	#make sure we have a screen ready, because the other tags are specific to a screen
	if screen is None:
		print("Error - using tag \"%s\" with value \"%s\", without a screen varaible - use the \"%s\" tag first to put this information on that screen." % (key, content, screen_tag))
		return
	
	text_boxes = screen["text_boxes"]
	
	#the actual text of the text box. this makes a new text box
	if key == text_tag:
		text_box = dict()
		text_box[text_tag] = content
		text_boxes.append(text_box)
		return
		
	#get the current text box for the current screen
	text_box = None
	if len(text_boxes) >= 1:
		text_box = text_boxes[-1]
	else:
		print("Error - trying to use tag \"%s\" with value \"%s\", without making a text box. Use the \"%s\" tag first." % (key, content, text_tag))
		return
	
	#x position. Can be a css value, or "centered"
	if key == x_tag:
		text_box[x_tag] = content
		return
	
	#y position. Is a css value.
	elif key == y_tag:
		text_box[y_tag] = content
		return
	
	#width of a text box. defaults to 20%
	elif key == width_tag:
		text_box[width_tag] = content
		return
		
	#direction of the dialogue box arrow (if anything)
	elif key == arrow_tag:
		text_box[arrow_tag] = content
		return
		
	
#read in a character's data
def read_player_file(filename):
	case_names = [s['key'] for s in situations]
	
	d = {}
	
	ending = dict()
	
	stage = -1
	
	f = open(filename, 'r')
	for line_number, line in enumerate(f):
		line = line.strip()
		
		line_data = dict() #all of the lines data:
		#key is the stage and situation in which the line should be used. includes a stage number for stage-specific lines
		#image = the image filename (if no extension, assumed to be png)
		#target = if the line targets a particular other character
		#targetStage = if the line targets a particular stage for a particular character
		#filter = if the line targets a particular tag
		
		if len(line) <= 0 or line[0]=='#': #use # as a comment character, and skip empty lines
			continue
		
		#check for characters that can't be used
		if sys.version_info[0] == 2:
			skip_line = False
			try:
				# In utf-8, characters using umlauts are actually encoded as two separate characters
				# so we need to try to decode the entire line instead of individual characters
				line.decode('utf-8')
			except UnicodeDecodeError:
				# Find out which character
				problem_character = ""
				for c in line:
					try:
						c.decode('utf-8')
					except UnicodeDecodeError:
						problem_character = c
						break

				if (len(problem_character) > 0):
					print("Unable to decode character %s in line %d: \"%s\"" % (problem_character, line_number, line))
				else:
					print("Unable to decode line \"%s\" in line %d: " % (line, line_number))

				skip_line = True
				break

			if skip_line:
				continue
		
		#split the lines, then check for malformed entries
		try:
			key, text = line.split("=", 1)
		except ValueError:
			#this helps to find lines that are misformed 
			print("Unable to split line %d: \"%s\"" % (line_number, line))
			continue
		
		key = key.strip().lower()
		
		text = unescapeHTML(text.strip())
		
		
		#now deal with any possible targets and filters
		target_type = "skip" #reset any previous target type. this should only be used if there's a target present, but setting it here just in case
		if ',' in key:
			target_parts = key.split(',')
			key = target_parts[0]
			targets = target_parts[1:]
			for t in targets:
			
				try:
					target_type, target_value = t.rsplit(":", 1)
				except ValueError:
					#make sure the target has a format we can understand
					print("Invalid targeting for line %d - \"%s\". Skipping line." % (line_number, line))
					target_type = "skip"
					text = ""
					target_value = "N/A"
				
				target_type = target_type.strip()
				target_value = target_value.strip()
				
				#make sure there's a target. Can I check the data here to make sure that a target is valid?
				if len(target_value) <= 0:
					print("No target value specified for line %d - \"%s\". Skipping line." % (line_number, line))
					target_type = skip
					text = ""
				
				#now actually process valid targets
				#valid targets
				if target_type in all_targets:
					line_data[target_type] = target_value
					
				elif target_type == "skip":
					#skip this target type
					pass

				elif target_type in ["marker", "direction", "location"]:
					line_data[target_type] = target_value
					pass

				elif target_type.startswith("count-") or target_type == "count":
					condition_filter = target_type[6::]
					if "conditions" not in line_data:
						line_data["conditions"] = [[condition_filter, target_value]]
					else: line_data["conditions"].append([condition_filter, target_value])
					
				elif target_type.startswith("test:"):
					test_expr = target_type[5::]
					if "tests" not in line_data:
						line_data["tests"] = [[test_expr, target_value]]
					else: line_data["tests"].append([test_expr, target_value])

				else:
					#unknown target type
					print("Error - unknown target type \"%s\" for line %d - \"%s\". Skipping line." % (target_type, line_number, line))
					text = "" #make the script skip this line
					
				if target_type == "targetstage":
					#print a warning if they used a targetStage without a target
					have_target = False
					for other_target_data in targets:
						if "target:" in other_target_data:
							have_target = True
							break
					if not have_target:
						print("Warning - using a targetStage for line %d - \"%s\" without using a target value" % (line_number, line))
		
		
		#if the key contains a -, it belongs to a specific stage
		if '-' in key:
			stg, part_key = key.rsplit('-', 1)
			
			#if it starts with a * use the current stage
			if stg[0] == '*':
				key = "%d-%s" % (stage, part_key)
			
			#negative numbers count from the end. -1 is finished, -2 is masturbating, -3 is nude. -4 is the last layer of clothing, and so on.
			#using negative numbers assumes that they are after all the clothes entries
			elif stg[0] == '-' and stg[1:].isdigit():
				key = "%d-%s" % (stage + 4 + int(stg), part_key)
		else:
			part_key = key
		
		#cases, these can be repeated
		if part_key in case_names:
		
			line_data["key"] = part_key
		
			if text == "" or text == ",":
				#if there's no entry, skip it.
				continue
				
			if ',' not in text:
				#img, desc = "", text
				line_data["image"] = ""
				line_data["text"] = text
			else:
				img,desc = text.split(",", 1) #split into (image, text) pairs
				line_data["image"] = img
				line_data["text"] = desc.strip()

			if line_data["text"].find('~silent~') == 0:
				line_data["text"] = ""
			else:
				line_data["text"] = fixupDialogue(line_data["text"])

			#print "adding line", line	
			
			if key in d:
				d[key].append(line_data) #add it to existing list of responses
			else:
				d[key] = [line_data] #make a new list of responses
				
		#clothes is a list
		elif key == "clothes":
			stage += 1
			if "clothes" in d:
				d["clothes"].append(text)
			else:
				d["clothes"] = [text]

	#intelligence is written as
	#   intelligence=bad
	#   intelligence=good,3
	#this means to start at bad intelligence and switch to good starting at stage 3
	#   The label can be changed in the same manner
		elif key in ("intelligence", "label"):
			parts = text.split(",", 1)
			(from_stage, value) = (0 if len(parts) == 1 else parts[1], parts[0])
			if key in d:
				d[key][from_stage] = value
			else:
				d[key] = {from_stage: value}

		#tags for the character i.e. blonde, athletic, cute
		#tags can be written as either:
		#	tag=blonde
		#	tag=athletic
		#or as
		#	tags=blond, athletic
		elif key == "tag":
			if "character_tags" in d:
				if not text in d["character_tags"]:
					d["character_tags"].append(text)
				else:
					print("Warning - duplicated tag: '%s'" % text)
			else:
				d["character_tags"] = [text]

		elif key == "tags":
			character_tags = [tag.strip() for tag in text.split(',')]
			if "character_tags" in d:
				d["character_tags"] = d["character_tags"] + character_tags
			else:
				d["character_tags"] = character_tags

		elif key == "marker":
			if "markers" in d:
				d["markers"].append(text)
			else:
				d["markers"] = [text]

		#write start lines last to first
		elif key == "start":
			if key in d:
				d[key].append(fixupDialogue(text))
			else:
				d[key] = [fixupDialogue(text)]

		#this tag relates to an ending squence
		#use a different function, because it's quite complicated
		elif key in ending_tags:
			handle_ending_string(key, text, ending, d)
		
		#other values are single lines. These need to be in the data, even if the value is empty
		else:
			d[key] = text
	
	#add the final ending (if it exists)
	add_ending(ending, d)
	
    #set default intelligence, if the writer doesn't set it
	if "intelligence" not in d:
		d["intelligence"] = [["0", "average"]]

	return d

#make the meta.xml file
def make_meta_xml(data, filename):
	o = Element("opponent")
	
	enabled = "true" if "enabled" not in data or data["enabled"] == "true" else "false"
	o.subElement("enabled", enabled)
	
	values = ["first","last","label","pic","gender","height","from","writer","artist","description","has_ending","layers","character_tags"]
	
	for value in values:
		content = ""
		if value in data:
			content = data[value]
		if value == "pic":
			if content == "":
				content = "0-calm"
			content += ".png"
		if value == "description":
			content = typographicalize(content)
		
		if value == "layers":
			#the number of layers of clothing is taken directly from the clothing data
			content = str(len(data["clothes"]))

		if value == "label":
			content = data["label"][0]
			
		if value == "has_ending":
			#say whether or not they have an ending based on whether they have any ending data or not
			content = "true" if "endings" in data else "false"

		if value == "character_tags":
			tags_elem = o.subElement("tags")
			character_tags = data["character_tags"]
			for tag in character_tags:
			       tags_elem.subElement("tag", tag)
		else:
			o.subElement(value, content)

	open(filename, 'w').write(o.serialize())

#make the marker.xml file
def make_markers_xml(data, filename):
	if "markers" in data:
		o = Element("markers")
		markers = data["markers"]
		for marker_data in markers:
			name, scope, desc = marker_data.split(",", 2)
			if scope == "public":
				scope = "Public"
			elif scope == "private":
				scope = "Private"
			o.subElement("marker", desc, [("name", name), ("scope",scope)])
		
		open(filename, 'w').write(o.serialize())

#read the input data, the write the xml files
def make_xml(player_filename, out_filename, meta_filename=None, marker_filename=None):
	get_situations_from_xml()
	player_dictionary = read_player_file(player_filename)
	write_xml(player_dictionary, out_filename)
	if meta_filename is not None:
		make_meta_xml(player_dictionary, meta_filename)
	if marker_filename is not None:
		make_markers_xml(player_dictionary, marker_filename)

#make the xml files using the given arguments
#python make_xml <character data file> <behaviour.xml output file> <meta.xml output file>
if __name__ == "__main__":
	if len(sys.argv) <= 1:
		print("Please give the name of the dialogue file to process into XML files")
		exit()
	behaviour_name = "behaviour.xml"
	meta_name = "meta.xml"
	marker_name = "markers.xml"
	if len(sys.argv) > 2:
		behaviour_name = sys.argv[2]
	if len(sys.argv) > 3:
		meta_name = sys.argv[3]
	if len(sys.argv) > 4:
		marker_name = sys.argv[4]
		
	make_xml(sys.argv[1], behaviour_name, meta_name, marker_name)



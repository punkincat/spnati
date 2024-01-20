/********************************************************************************
 This file contains all of the variables and functions for the Player object, as
 well as definitions for opponent and group lisitings.
 ********************************************************************************/

/**********************************************************************
 * Enumerations
 **********************************************************************/

/************************************************************
 * An enumeration for gender.
 **/
var eGender = {
    MALE   : "male",
    FEMALE : "female"
};

/************************************************************
 * An enumeration for player size.
 **/
var eSize = {
    SMALL  : "small",
    MEDIUM : "medium",
    LARGE  : "large"
};

/************************************************************
 * An enumeration for player intelligence.
 **/
var eIntelligence = {
    NOSWAP  : "no-swap",
    THROW   : "throw",
    BAD     : "bad",
    AVERAGE : "average",
    GOOD    : "good",
    BEST    : "best"
};

/**********************************************************************
 *****                Player Object Specification                 *****
 **********************************************************************/

 /************************************************************
  * Creates and returns a new player object based on the
  * supplied information.
  *
  * folder (string), the path to their folder
  * first (string), their first name.
  * last (string), their last name.
  * labels (string or XML element), what's shown on screen and what other players refer to them as.
  *   Can vary by stage.
  * size (string): Their level of endowment
  * intelligence (string or XML element), the name of their AI algorithm.
  *   Can vary by stage.
  * gender (constant), their gender.
  * clothing (array of Clothing objects), their clothing.
  * stamina (integer), time until forfeit is finished (initial timer value).
  * state (array of PlayerState objects), their sequential states.
  * xml (jQuery object), the player's loaded behaviour.xml file.
  * metaXml (jQuery object), the player's loaded meta.xml file.
  ************************************************************/

function Player (id) {
    this.id = id;
    this.folder = 'opponents/'+id+'/';
    this.base_folder = 'opponents/'+id+'/';
    this.first = '';
    this.last = '';
    this.labels = undefined;
    this.folders = undefined;
    this.size = eSize.MEDIUM;
    this.intelligence = eIntelligence.AVERAGE;
    this.gender = eGender.MALE;
    this.stamina = 20;
    this.scale = undefined;
    this.tags = this.baseTags = [];
    this.xml = null;
    this.persistentMarkers = {};
}

/*******************************************************************
 * Sets initial values of state variables used by targetStatus,
 * targetStartingLayers etc. adccording to wardrobe.
 *******************************************************************/
Player.prototype.initClothingStatus = function () {
    this.startingLayers = this.countLayers();
    this.numStripped = { extra: 0, minor: 0, major: 0, important: 0 };
    this.mostlyClothed = this.isDecent();
}

/********************************************************************
 * Gets the currently worn wardrobe, possible before (stageDelta ==
 * -1) or after (stageDelta == 1) stripping removedClothing.
 ********************************************************************/
Player.prototype.getClothing = function(stageDelta, removedClothing) {
    removedClothing ||= this.removedClothing;
    return this.clothing.filter(c =>
        c.type != 'skip'
            && (!c.removed || (stageDelta == -1 && c == removedClothing))
            && (stageDelta != 1 || c != removedClothing)
            && (c.fromStage === undefined
                || (stageDelta == 1 && c.fromStage == this.stage + 1 && !c.fromDeal)));
};

/*******************************************************************
 * (Re)Initialize the player properties that change during a game
 *******************************************************************/
Player.prototype.resetState = function () {
    this.out = this.finished = false;
    this.outOrder = undefined;
    this.biggestLead = 0;
    this.forfeit = "";
    this.forfeitLocked = false;
    this.finishingTarget = this;
    this.stage = this.consecutiveLosses = 0;
    this.timeInStage = 0;
    this.ticksInStage = 0;
    this.markers = {};
    this.saidDialogue = {};
    this.hand = null;

    /** @type {{[tag: string]: {present: boolean, stage: number}}} */
    this.tagModifications = {};

    if (this.xml !== null) {
        /* Initialize reaction handling state. */
        this.currentTarget = null;
        this.currentTriggers = [];
        this.stateCommitted = false;

        this.oneShotCases = {};
        this.oneShotStates = {};

        var appearance = this.default_costume;
        if (this.alt_costume) {
            appearance = this.alt_costume;
        }

        this.labels = appearance.labels;
        this.folders = appearance.folders;
        this.baseTags = appearance.tags.slice();
        this.labelOverridden = this.intelligenceOverridden = false;

        /* The gender listed in meta.xml and behaviour.xml might differ
         * (for example with gender-revealing characters)
         * So assume behaviour.xml holds the 'definitive' starting gender
         * for the character.
         */
        this.gender = appearance.gender;
        this.size = appearance.size;

        this.stamina = Number(this.xml.children('timer').text());

        /* Clear the repeat log between games. */
        this.repeatLog = {};

        /* Load the player's wardrobe. */

        /* Find and grab the wardrobe tag */
        $wardrobe = appearance.wardrobe;

        this.settings.forEach((group) => group.reset());

        /* find and create all of their clothing */
        var clothingArr = [];
        $wardrobe.children('clothing').each(function () {
            var generic = $(this).attr('generic');
            var name = $(this).attr('name');
            var type = $(this).attr('type');
            var position = $(this).attr('position');
            var plural = $(this).attr('plural');
            plural = (plural == 'null' ? null : plural == 'true');

            var newClothing = new Clothing(name, generic, type, position, plural);

            clothingArr.push(newClothing);
        });

        this.clothing = clothingArr;
        this.initClothingStatus();

        this.loadStylesheet();
        this.stageChangeUpdate();
    }
}

/* These shouldn't do anything for the human player, but exist as empty functions
   to make it easier to iterate over the entire players[] array. */
Player.prototype.updateLabel = function () { }
Player.prototype.updateIntelligence = function () { }
Player.prototype.updateFolder = function () { }
Player.prototype.updateBehaviour = function() { }
Player.prototype.singleBehaviourUpdate = function() { }

/**********************************************************************
 * Convert a tags list to canonical form:
 * - Canonicalize each input tag
 * - Resolve tag implications
 * This function also filters out duplicated tags.
 **********************************************************************/
Player.prototype.expandTagsList = function(input_tags) {
    let tmp = input_tags.map(canonicalizeTag);
    let output_tags = [];

    while (tmp.length > 0) {
        let tag = tmp.shift();

        // Ensure exactly one instance of each tag remains within the output array.
        if (output_tags.indexOf(tag) >= 0) continue;
        output_tags.push(tag);

        // If this tag implies other tags, queue those for processing as well.
        if (TAG_IMPLICATIONS.hasOwnProperty(tag)) {
            Array.prototype.push.apply(tmp, TAG_IMPLICATIONS[tag]);
        }
    }

    /* "chubby" implies "curvy" on female characters only,
       so this has to be done separately */
    if (output_tags.includes("chubby") && this.gender == eGender.FEMALE) {
        output_tags.push("curvy");
    }

    return output_tags;
}

/* Compute the Player's tags list from their baseTags list. */
Player.prototype.updateTags = function () {
    var tags = [this.id];
    var stage = this.stage || 0;

    if (this.alt_costume && this.alt_costume.id) {
        tags.push(this.alt_costume.id);
    }

    this.baseTags.forEach(function (tag_desc) {
        if (typeof(tag_desc) === 'string') {
            tags.push(tag_desc);
            return;
        }

        if (!tag_desc.tag) return;

        var tag = tag_desc.tag;
        var from = parseInt(tag_desc.from, 10);
        var to = parseInt(tag_desc.to, 10);

        if (isNaN(to))   to = Number.POSITIVE_INFINITY;
        if (isNaN(from)) from = 0;

        if (stage >= from && stage <= to) {
            tags.push(tag);
        }
    });

    this.tags = this.expandTagsList(tags);
}

Player.prototype.stageChangeUpdate = function () {
    this.updateLabel();
    this.updateIntelligence();
    this.updateFolder();
    this.updateTags();
}

Player.prototype.addTag = function(tag) {
    if (tag) {
        this.tagModifications[tag] = {present: true, stage: this.stage};
        this.baseTags.push(canonicalizeTag(tag));
    }
}

Player.prototype.removeTag = function(tag) {
    tag = canonicalizeTag(tag);

    if (this.hasTag(tag)) {
        this.tagModifications[tag] = {present: false, stage: this.stage};
        this.baseTags = this.baseTags.filter(function (t) {
            if (typeof(t) === 'string') { return t !== tag };
            if (!t.tag) return false;
            return t.tag !== tag;
        });
    }
}

Player.prototype.hasTag = function(tag) {
    if (tag && tag[0] == "!") {
        return !this.hasTag(tag.substring(1));
    }

    return tag && this.tags && this.tags.indexOf(canonicalizeTag(tag)) >= 0;
};

Player.prototype.hasTags = function(tagAdv) {
    return tagAdv.split('\|').some(subs => subs.split('&').every(expr => this.hasTag(expr)));
}

Player.prototype.countLayers = function() {
    return this.clothing.countTrue(c => !c.removed && c.type != "skip");
};

Player.prototype.checkStatus = function(status) {
    if (status.substr(0, 4) == "not_") {
        return !this.checkStatus(status.substr(4));
    }
    switch (status.trim()) {
    case STATUS_LOST_SOME:
        return this.stage > 0;
    case STATUS_MOSTLY_CLOTHED:
        return this.mostlyClothed;
    case STATUS_DECENT:
        return this.isDecent();
    case STATUS_EXPOSED_TOP:
        return !this.isCovered(UPPER_ARTICLE);
    case STATUS_EXPOSED_BOTTOM:
        return !this.isCovered(LOWER_ARTICLE);
    case STATUS_EXPOSED:
        return !this.isCovered(UPPER_ARTICLE) || !this.isCovered(LOWER_ARTICLE);
    case STATUS_EXPOSED_TOP_ONLY:
        return !this.isCovered(UPPER_ARTICLE) && this.isCovered(LOWER_ARTICLE);
    case STATUS_EXPOSED_BOTTOM_ONLY:
        return this.isCovered(UPPER_ARTICLE) && !this.isCovered(LOWER_ARTICLE);
    case STATUS_NAKED:
        return !this.isCovered(UPPER_ARTICLE) && !this.isCovered(LOWER_ARTICLE);
    case STATUS_ALIVE:
        return !this.out;
    case STATUS_LOST_ALL:
        return this.countLayers() == 0;
    case STATUS_MASTURBATING:
        return this.out && !this.finished;
    case STATUS_HEAVY_MASTURBATING:
        return this.out && !this.finished && this.forfeit[0] === PLAYER_HEAVY_MASTURBATING;
    case STATUS_FINISHED:
        return this.finished;
    }
}

/**
 * Get the value of a marker set on this Player.
 *
 * This method always attempts to parse stored marker values as integers,
 * but if this isn't possible then the raw string will be returned instead
 * (unless `numeric` is set to `true`).
 *
 * If a `target` is passed, the marker value will be read from a per-target
 * marker first, if possible. By default (if `targeted_only` is not `true`),
 * if the per-target marker is not found, the base marker's value will be
 * used as a default.
 *
 * @param {string} baseName The name of the marker to look up.
 * @param {Player} target If passed, the value will be loaded on a per-target
 * basis.
 * @param {boolean} numeric If `true`, then stored marker values that cannot
 * be converted to number values will be returned as 0 instead of as strings.
 * @param {boolean} targeted_only If `true`, then per-target markers will
 * _not_ default to using their base names if not found.
 * @returns {number | string}
 */
Player.prototype.getMarker = function (baseName, target, numeric, targeted_only) {
    var val = 0;

    var name = baseName;
    if (target && target.id) {
        name = getTargetMarker(baseName, target);
    }

    if (!this.markers) {
        console.error("Marker object not initialized for opponent " + this.id, this);
        console.trace();

        /* This might be a bad idea, since if we get here, then resetState()
         * must not have been called for some reason.
         * But better this than crashing... maybe?
         */
        this.markers = {};
    }

    if (!this.persistentMarkers[baseName]) {
        val = this.markers[name];

        if (!val && target && !targeted_only) {
            /* If the per-target marker wasn't found, attempt to default
             * to the nonspecific marker.
             */
            val = this.markers[baseName];
        }
    } else {
        val = save.getPersistentMarker(this, name);

        if (!val && target && !targeted_only) {
            val = save.getPersistentMarker(this, baseName);
        }
    }

    var cast = parseInt(val, 10);

    if (!isNaN(cast)) {
        return cast;
    } else if (numeric) {
        return 0;
    } else {
        return val;
    }
}

/**
 * Set the value of a marker on this Player.
 *
 * @param {string} baseName The name of the marker to set.
 * @param {Player} target If passed, the value will be set on a per-target
 * basis.
 * @param {string | number} value The value to set for the marker.
 */
Player.prototype.setMarker = function (baseName, target, value) {
    var name = baseName;
    if (target && target.id) {
        name = getTargetMarker(baseName, target);
    }

    if (!this.persistentMarkers[baseName]) {
        this.markers[name] = value;
    } else {
        save.setPersistentMarker(this, name, value);
    }
}

/**
 * Calculates how many lines from currently-selected characters target this
 * character.
 *
 * @param {string} [filterStatus] If passed, only lines from characters with the
 * given status will be considered.
 * 
 * @param {number} [cap] If passed, each currently selected character's contribution
 * to the total inbound line count will be capped to this number.
 *
 * @returns {number}
 */
Player.prototype.inboundLinesFromSelected = function (filterStatus, cap) {
    var id = this.id;

    return players.reduce(function(sum, p) {
        if (p && p.targetedLines && id in p.targetedLines
            && (!filterStatus || p.status === filterStatus)) {
            if (cap) {
                sum += Math.min(p.targetedLines[id].seen.size, cap);
            } else {
                sum += p.targetedLines[id].seen.size;
            }
        }

        return sum;
    }, 0);
}

/**
 * Given a pose name, resolve it to either a custom pose,
 * a pose set, or an image file path.
 * 
 * If this function returns a string, it is a full image file path
 * that is ready to be used as e.g. the `src` attribute for an `<img>` element.
 * 
 * @param {string} image 
 * @param {number?} stage
 * @returns {string | PoseDefinition | PoseSet}
 */
Player.prototype.resolvePoseName = function (image, stage) {
    if (!image) {
        return null;
    }

    if (stage === null || stage === undefined) {
        stage = this.stage;
    }

    image = image.replace("#", stage);
    if (image.startsWith("custom:") && this.poses) {
        let key = image.substring(7);
        return this.poses[key];
    } else if (image.startsWith("set:") && this.poseSets) {
        let key = image.substring(4);
        return this.poseSets[key];
    } else {
        return getActualSpriteSrc(image, this, stage);
    }
}

/**
 * Subclass of Player for AI-controlled players.
 *
 * @constructor
 *
 * @param {string} id
 * @param {jQuery} $metaXml
 * @param {string} status
 * @param {number} [rosterScore]
 * @param {string} [addedDate]
 * @param {number} [releaseNumber]
 * @param {string} [highlightStatus]
 */
function Opponent (id, metaFiles, status, rosterScore, addedDate, releaseNumber, highlightStatus) {
    Player.call(this, id);

    this.id = id;
    this.folder = 'opponents/'+id+'/';
    this.base_folder = 'opponents/'+id+'/';
    
    var $metaXml = metaFiles[0];
    var $tagsXml = metaFiles[1];

    this.status = status;
    this.highlightStatus = eventCharacterSettings.highlights[id] ||  highlightStatus || status || '';
    this.first = $metaXml.children('first').text();
    this.last = $metaXml.children('last').text();

    // For label and gender, track the original, default value from
    // meta.xml, the value for the currently selected costume to be
    // shown on the selection card, and the current in-game value.
    this.label = this.selectLabel = this.metaLabel = $metaXml.children('label').text();
    this.gender = this.selectGender = this.metaGender = $metaXml.children('gender').text();

    var picElem = $metaXml.children('pic');

    this.image = picElem.text();
    this.source = $metaXml.children('from').text();
    this.artist = $metaXml.children('artist').text();
    this.writer = $metaXml.children('writer').text();
    this.description = fixupDialogue($metaXml.children('description').html());
    this.has_collectibles = $metaXml.children('has_collectibles').text() === "true";
    this.collectibles = null;
    this.layers = this.selectLayers = this.metaLayers = parseInt($metaXml.children('layers').text(), 10);
    this.default_costume_name = $metaXml.children('default-costume-name').text();
    this.scale = Number($metaXml.children('scale').text()) || 100.0;
    this.release = releaseNumber;
    this.uniqueLineCount = parseInt($metaXml.children('lines').text(), 10) || undefined;
    this.posesImageCount = parseInt($metaXml.children('poses').text(), 10) || undefined;
    this.z_index = parseInt($metaXml.children('z-index').text(), 10) || 0;
    this.dialogue_layering = $metaXml.children('dialogue-layer').text();
    this.fontSize = $metaXml.children('font-size').text();
    if (!['small', 'smaller'].includes(this.fontSize)) this.fontSize = undefined;
    this.lastUpdated = parseInt($metaXml.children('lastupdate').text(), 10) || 0;

    /* For sorting purposes. 
     * Simplifies diacritics (to solve the Pokemon problem), removes punctuation,
     * removes some basic stop words (and, the, a, an), and also removes
     * parentheticals.
     */
    this.normalizedSource = (this.source || "")
        .simplifyDiacritics()
        .replace(/\([^\)]+\)|\[[^\]]+\]|and\s|the\s|an?\s|[^a-zA-Z0-9]/gm, '');

    this.normalizedSelectLabel = (this.selectLabel || "")
        .simplifyDiacritics()
        .replace(/[^a-zA-Z0-9]/gm, '');

    this.rosterScore = rosterScore;
    this.effectiveScore = -Infinity;

    this.addedDate = addedDate;

    this.endings = null;
    if (EPILOGUES_ENABLED) {
        var $endings = $metaXml.children('epilogue').filter(function (idx, elem) {
            var status = $(elem).attr('status');
            return (!status || includedOpponentStatuses[status]);
        }.bind(this));
        if ($endings.length) {
            this.endings = $endings;
        }
    }

    if (['over', 'under'].indexOf(this.dialogue_layering) < 0) {
        this.dialogue_layering = 'under';
    }

    this.selected_costume = null;
    this.alt_costume = null;
    this.default_costume = null;
    this.poses = {};
    this.poseSets = {};
    this.imageCache = {};
    this.labelOverridden = this.intelligenceOverridden = false;
    this.pendingCollectiblePopups = [];
    this.repeatLog = {};
    this.settings = [];

    this.loaded = false;
    this.loadProgress = undefined;
    this.selectInfo = null;

    /* originalTags stores tags that will be later used in resetState to build the
     * opponent's true tags list. It does not store implied tags.
     *
     * The tags list stores the fully-expanded list of tags for the opponent,
     * including implied tags.
     */
    this.originalTags = $tagsXml.find('>tags>tag').map(function () {
        return {
            'tag': canonicalizeTag($(this).text()),
            'from': $(this).attr('from'),
            'to': $(this).attr('to'),
        }
    }).get();
    this.searchTags = this.expandTagsList(this.originalTags.map(obj => obj.tag));
	
    this.magnetismTag = undefined;
    this.searchTags.forEach((tag) => {
        if (MAGNET_TAGS.indexOf(tag) >= 0) this.magnetismTag = tag;
    });

    this.cases = new Map();

    /* Attempt to preload this opponent's picture for selection. */
    new Image().src = 'opponents/'+id+'/'+this.image;

    this.alternate_costumes = [];
    this.selection_image = this.folder + this.image;
    this.selection_image_adjustment = {
        x: (Number(picElem.attr("x")) || 0), /* negative values move to the left, positive to the right */
        y: (-Number(picElem.attr("y")) || 0), /* negative values move down, positive moves up */
        scale: Number(picElem.attr("scale")) || 100.0,
    };

    this.favorite = save.isCharacterFavorited(this);

    this.event_character = eventCharacterSettings.ids.has(id);
    this.event_sort_order = (
        (eventCharacterSettings.sorting[id] !== undefined) ? eventCharacterSettings.sorting[id]
        : (eventCharacterSettings.ids.has(id) ? 1 : 0)
    );
    this.event_partition = eventCharacterSettings.partitions[id] || 0;
    this.force_prefill = (eventCharacterSettings.prefills[id] !== undefined) ? eventCharacterSettings.prefills[id] : false;
    this.allow_testing_guest = (eventCharacterSettings.allowTestingGuests[id] !== undefined) ? eventCharacterSettings.allowTestingGuests[id] : false;

    this.matchesEventTag = false;
    eventTagList.some(function (tag) {
        if (this.searchTags.indexOf(tag) >= 0) {
            this.matchesEventTag = true;
            this.event_character = true;
            if (eventTagSettings.highlights[tag] && !eventCharacterSettings.highlights[id]) {
                this.highlightStatus = eventTagSettings.highlights[tag];
            }

            if (eventCharacterSettings.sorting[id] === undefined) {
                this.event_sort_order = (eventTagSettings.sorting[tag] !== undefined) ? eventTagSettings.sorting[tag] : 2;
            }

            if (eventCharacterSettings.partitions[id] === undefined && eventTagSettings.partitions[tag] !== undefined) {
                /* The default partition value in all cases is 0, so if eventTagSettings.partitions[tag] === undefined, we don't need to do anything. */
                this.event_partition = eventTagSettings.partitions[tag];
            }

            if (eventCharacterSettings.prefills[id] === undefined) {
                if (eventTagSettings.ids.has(tag) && eventTagSettings.prefills[tag] === undefined) {
                    this.force_prefill = true;
                } else {
                    this.force_prefill = eventTagSettings.prefills[tag];
                }
            }

            if (eventCharacterSettings.allowTestingGuests[id] === undefined && eventTagSettings.allowTestingGuests[tag] !== undefined) {
                this.allow_testing_guest = eventTagSettings.allowTestingGuests[tag];
                // The default value of allowTestingGuests for specific characters and for tagged characters is false, so if the tag
                // has no specified attribute value, we don't need to do anything.
            }

            return true;
        }
        return false;
    }.bind(this));

    if (this.event_sort_order !== 0 || this.event_partition !== 0) eventSortingActive = true;

    if (!ALT_COSTUMES_ENABLED) return;

    var defaultCostumes = [];
    $metaXml.find('>alternates>costume').each(function (i, elem) {
        var set = $(elem).attr('set');
        var status = $(elem).attr('status') || 'online';

        if ((set === undefined || alternateCostumeSets['all'] || alternateCostumeSets[set]) && includedOpponentStatuses[status]) {
            var costume_descriptor = {
                'folder': $(elem).attr('folder'),
                'name': $(elem).text(),
                'image': $(elem).attr('img'),
                'gender': $(elem).attr('gender') || this.selectGender,
                'label': $(elem).attr('label') || this.selectLabel,
                'set': set,
                'status': status,
                'unlocked_by': $(elem).attr('collectible') || '',
                'layers': parseInt($(elem).attr('layers'), 10) || this.selectLayers,
            };

            if (set && DEFAULT_COSTUME_SETS.has(set)) {
                defaultCostumes.push(costume_descriptor);
            }

            this.alternate_costumes.push(costume_descriptor);
        }
    }.bind(this)).get();

    this.hasDefaultCostume = defaultCostumes.length > 0;
    if (this.hasDefaultCostume) {
        var selectedDefault = defaultCostumes[getRandomNumber(0, defaultCostumes.length)];
        var costumeSet = selectedDefault.set;

        this.selection_image = selectedDefault['folder'] + selectedDefault['image'];
        this.selectAlternateCostume(selectedDefault);

        if (eventCostumeSettings.ids.has(costumeSet)) {
            this.event_character = true;
            
            if (eventCostumeSettings.highlights[costumeSet] && !eventCharacterSettings.highlights[id]) {
                this.highlightStatus = eventCostumeSettings.highlights[costumeSet];
            }
    
            if (eventCharacterSettings.sorting[id] === undefined) {
                if (eventCostumeSettings.sorting[costumeSet] !== undefined) {
                    this.event_sort_order = eventCostumeSettings.sorting[costumeSet];
                } else if (!this.matchesEventTag) {
                    this.event_sort_order = 3;
                }

                if (this.event_sort_order != 0) eventSortingActive = true;
            }
    
            if (eventCharacterSettings.partitions[costumeSet] === undefined && eventCostumeSettings.partitions[costumeSet] !== undefined) {
                this.event_partition = eventCostumeSettings.partitions[costumeSet];
                if (this.event_partition != 0) eventSortingActive = true;
            }

            if (eventCharacterSettings.prefills[id] === undefined) {
                if (eventCostumeSettings.prefills[costumeSet] !== undefined) {
                    this.force_prefill = eventCostumeSettings.prefills[costumeSet];
                } else if (!this.matchesEventTag) {
                    this.force_prefill = true;
                }
                // If an event tag is matched, fall back to the value used there
            }

            if (eventCharacterSettings.allowTestingGuests[id] === undefined) {
                if (eventCostumeSettings.allowTestingGuests[costumeSet] !== undefined) {
                    this.allow_testing_guest = eventCostumeSettings.allowTestingGuests[costumeSet];
                } else if (!this.matchesEventTag) {
                    this.allow_testing_guest = false;
                }
            }
        }
    }

    // Not reached if alt costumes are disabled
}

Opponent.prototype = Object.create(Player.prototype);
Opponent.prototype.constructor = Opponent;

Opponent.prototype.clone = function() {
    var clone = Object.create(Opponent.prototype);
    /* This should be deep enough for our purposes. */
    for (var prop in this) {
        if (this[prop] instanceof Array) {
            clone[prop] = this[prop].slice();
        } else {
            clone[prop] = this[prop];
        }
    }
    return clone;
}

Opponent.prototype.isLoaded = function() {
    return this.loaded;
}

Opponent.prototype.onSelected = function(individual) {
    this.resetState();
    console.log(this.slot+": ");
    console.log(this);

    Sentry.addBreadcrumb({
        category: 'select',
        message: 'Load completed for ' + this.id,
        level: 'info'
    });

    this.loaded = true;

    this.preloadStageImages(-1);
    if (individual) {
        updateAllBehaviours(this.slot, SELECTED, [[OPPONENT_SELECTED]]);
    } else {
        this.singleBehaviourUpdate(SELECTED, null);
    }

    updateSelectionVisuals();
}

/**
 * Load this character's stylesheet normally (using a <link> element), if not already loaded.
 */
Opponent.prototype.loadStylesheet = function () {
    /* check for duplicate <link> elements and hot-reloaded stylesheets: */
    if (this.stylesheet) {
        if ($('link[href=\"'+this.stylesheet+'\"]').length === 0 && !this.liveStyleElem) {
            console.log("Loading stylesheet: "+this.stylesheet);

            var link_elem = $('<link />', {
                'rel': 'stylesheet',
                'type': 'text/css',
                'href': this.stylesheet
            });

            $('head').append(link_elem);
        }
    }
}

/**
 * Reload a character's stylesheet from the origin.
 * This is used when hot-reloading epilogues for debugging purposes.
 * 
 * Unlike the regular `loadStylesheet` method, this loads the contents of the character's
 * stylesheet as an injected `<style>` element to circumvent caching and force a complete
 * reload of all styling rules contained within the sheet.
 * 
 * Returns a promise that resolves when the new stylesheet has been fetched and injected into the page.
 * 
 * @returns {Promise<void>}
 */
Opponent.prototype.hotReloadStylesheet = function () {
    this.unloadStylesheet();

    if (!this.stylesheet) {
        return immediatePromise();
    }

    return fetch(this.stylesheet, { method: "GET" }).then((resp) => {
        if (resp.status < 200 || resp.status > 299) {
            throw new Error("Fetching " + url + " failed with error " + resp.status + ": " + resp.statusText);
        } else {
            return resp.text();
        }
    }).then((styleContents) => {
        this.liveStyleElem = document.createElement("style");
        this.liveStyleElem.innerHTML = styleContents;
        $("head").append(this.liveStyleElem);
    });
}

/**
 * Unloads this character's stylesheet from the page.
 */
Opponent.prototype.unloadStylesheet = function () {
    if (this.stylesheet) {
        /* Remove the <link> to this opponent's stylesheet. */
        $('link[href=\"'+this.stylesheet+'\"]').remove();
    }

    if (this.liveStyleElem) {
        $(this.liveStyleElem).remove();
        this.liveStyleElem = null;
    }
}

Opponent.prototype.updateLabel = function () {
    if (!this.labelOverridden) {
        if (this.labels && this.labels.length) {
            this.label = this.getByStage(this.labels);
        } else {
            this.label = this.selectLabel;
        }
    }
}

Opponent.prototype.setLabel = function(label) {
    if (label) {
        this.label = label;
        this.labelOverridden = true;
    } else if (label !== undefined) {
        this.labelOverridden = false;
        this.updateLabel();
    }
}

Opponent.prototype.updateIntelligence = function () {
    if (!this.intelligenceOverridden) {
        if (this.intelligences && this.intelligences.length) {
            this.intelligence = this.getByStage(this.intelligences);
        }
        if (!this.intelligence) {
            this.intelligence = eIntelligence.AVERAGE;
        }
    }
}

Opponent.prototype.setIntelligence = function (intelligence) {
    if (intelligence) {
        this.intelligence = intelligence;
        this.intelligenceOverridden = true;
    } else if (intelligence !== undefined) {
        this.intelligenceOverridden = false;
        this.updateIntelligence();
    }
}

Opponent.prototype.updateFolder = function () {
    if (this.folders) this.folder = this.getByStage(this.folders);
    if (!this.folder) {
        /* Shouldn't happen, but... */
        captureError(new Error(
            "Could not find folder for " + this.id + " at stage " + this.stage +
            (this.selected_costume ? " with alt costume " + this.selected_costume : "")
        ));

        this.folder = this.selected_costume || this.base_folder;
    }

    if (this.folder == this.base_folder) {
        this.poses = this.default_costume.poses;
        this.poseSets = this.default_costume.poseSets;
    } else if (this.alt_costume) {
        this.poses = this.alt_costume.poses;
        this.poseSets = this.alt_costume.poseSets;
    }
}

Opponent.prototype.getByStage = function (arr, stage) {
    if (typeof(arr) === "string") {
        return arr;
    }
    if (stage === undefined) stage = this.stage;
    var bestFitStage = -1;
    var bestFit = null;
    for (var i = 0; i < arr.length; i++) {
        var startStage = arr[i].getAttribute('stage');
        startStage = parseInt(startStage, 10) || 0;
        if (startStage > bestFitStage && startStage <= stage) {
            bestFit = $(arr[i]).text();
            bestFitStage = startStage;
        }
    }
    return bestFit;
};

/**
 * Get the repeat count for the currently displayed line, if any.
 * @returns {number}
 */
Opponent.prototype.getRepeatCount = function () {
    if (!this.chosenState || !this.chosenState.rawDialogue) {
        return 0;
    }

    return this.repeatLog[this.chosenState.rawDialogue] || 0;
}

/**
 * Mark this character as favorited or not.
 * @param {boolean} value 
 */
Opponent.prototype.setFavorited = function (value) {
    this.favorite = value;
    save.setCharacterFavorited(this, value);
    updateIndividualSelectSort();
}

Opponent.prototype.selectAlternateCostume = function (costumeDesc) {
    if (!costumeDesc) {
        this.selected_costume = null;
        this.selection_image = this.base_folder + this.image;
        this.selectLabel = this.metaLabel;
        this.selectGender = this.metaGender;
        this.selectLayers = this.metaLayers;
    } else {
        this.selected_costume = costumeDesc.folder;
        this.selection_image = costumeDesc.folder + costumeDesc.image;
        this.selectLabel = costumeDesc.label;
        this.selectGender = costumeDesc.gender;
        this.selectLayers = costumeDesc.layers;
    }

    /* For sorting purposes. 
     * Simplifies diacritics and removes punctuation.
     */
    this.normalizedSelectLabel = (this.selectLabel || "")
        .simplifyDiacritics()
        .replace(/[^a-zA-Z0-9]/gm, '');
    

    if (this.selectionCard)
        this.selectionCard.update();
};

/**
 * Loads and parses the selected alternate costume for this opponent.
 * 
 * @returns {Promise<void>} A Promise that resolves after all loading for the
 * selected costume is complete.
 * 
 * @throws The returned Promise will reject if the costume data cannot be fetched
 * or if an error is encountered during loading.
 */
Opponent.prototype.loadAlternateCostume = function () {
    if (this.alt_costume) {
        if (this.alt_costume.folder != this.selected_costume) {
            this.unloadAlternateCostume();
        } else {
            return immediatePromise();
        }
    }

    console.log("Loading alternate costume: "+this.selected_costume);
    this.loaded = false;

    return metadataIndex.getFile(this.selected_costume+'costume.xml').then(function ($xml) {
        Sentry.addBreadcrumb({
            category: 'select',
            message: 'Initializing alternate costume for ' + this.id + ': ' + this.selected_costume,
            level: 'info'
        });

        this.alt_costume = {
            id: $xml.children('id').text(),
            labels: $xml.children('label'),
            tags: [],
            folder: this.selected_costume,
            folders: $xml.children('folder'),
            wardrobe: $xml.children('wardrobe'),
            gender: $xml.children('gender').text() || this.selectGender,
            size: $xml.children('size').text() || this.default_costume.size,
            layers: parseInt($xml.children('layers').text(), 10) || this.selectLayers,
        };

        var poses = $xml.children('poses');
        var poseDefs = {};
        Object.assign(poseDefs, this.default_costume.poses);
        $(poses).children('pose').each(function (i, elem) {
            var def = new PoseDefinition($(elem), this);
            poseDefs[def.id] = def;
        }.bind(this));

        var setElems = $xml.children('pose-sets');
        var poseSets = {};
        Object.assign(poseSets, this.default_costume.poseSets);
        $(setElems).children("set").each(
            (i, elem) => {
                let parsed = PoseSet.parseXML(this, $(elem));
                poseSets[parsed.id] = parsed;
            }
        );

        this.alt_costume.poses = poseDefs;
        this.alt_costume.poseSets = poseSets;

        var costumeTags = this.default_costume.tags.slice();
        var tagMods = $xml.children('tags');
        if (tagMods) {
            var newTags = [];
            tagMods.children('tag').each(function (idx, elem) {
                var $elem = $(elem);
                var tag = canonicalizeTag($elem.text());
                var removed = $elem.attr('remove') || '';
                var fromStage = $elem.attr('from');
                var toStage = $elem.attr('to');

                // Remove previous declarations for this tag
                costumeTags = costumeTags.filter(function (t) { return t.tag !== tag; });

                if (removed.toLowerCase() !== 'true') {
                    newTags.push({'tag': tag, 'from': fromStage, 'to': toStage});
                }
            });

            Array.prototype.push.apply(costumeTags, newTags);
        }

        this.alt_costume.tags = costumeTags;
    }.bind(this)).catch(function (err) {
        console.error("Failed to load alternate costume: "+this.selected_costume);
        throw err;
    }.bind(this));
}

Opponent.prototype.unloadAlternateCostume = function () {
    if (!this.alt_costume) {
        return;
    }

    this.alt_costume = null;
    this.resetState();
}

/**
 * Load the collectibles for this opponent by fetching collectibles.xml if necessary.
 * 
 * @returns {Promise<void>} A Promise that resolves after all collectibles are
 * loaded.
 * 
 * @throws The returned Promise will reject if the collectibles for this character
 * cannot be fetched or if loading them causes an error.
 */
Opponent.prototype.fetchCollectibles = function () {
    if (!this.has_collectibles || this.collectibles !== null) {
        return immediatePromise();
    }

    console.log("Fetching collectibles for " + this.id);

    return metadataIndex.getFile(this.folder + "collectibles.xml").then(function ($xml) {
        var collectiblesArray = [];
        $xml.children('collectible').each(function (idx, elem) {
            collectiblesArray.push(new Collectible($(elem), this));
        }.bind(this));

        this.collectibles = collectiblesArray;
        this.has_collectibles = this.collectibles.some(function (c) {
            return !c.status || includedOpponentStatuses[c.status];
        });
    }.bind(this)).catch(function (err) {
        console.error("Error loading collectibles for "+this.id);
        throw err;
    }.bind(this));
}

Opponent.prototype.listUnlockedCostumes = function () {
    let unlocked_costumes = [];
    let thisOpponent = this; 
    this.alternate_costumes.map(function(costume) {
        if (costume.unlocked_by == '')
        {
            unlocked_costumes.push(costume);
        }
        else if (thisOpponent.collectibles.some(
            function (collectible) { if(collectible.id === costume.unlocked_by) {return collectible.isUnlocked();} else return false;}))
            {
                unlocked_costumes.push(costume);
            }      
    });
    return unlocked_costumes;
}

/**
 * Get quick details on epilogue conditions and whether they're met, given
 * the current player gender and table composition.
 */
Opponent.prototype.getAllEpilogueStatus = function () {
    if (!this.endings) {
        return [];
    }

    var ret = [];
    this.endings.each(function (idx, elem) {
        var $elem = $(elem);

        var summary = {
            title: $elem.text(),
            extraConditions: false,
            wrongGender: false,
            requiredCharacters: null,
            characterIsMissing: false,
            hint: undefined,
            description: undefined,
        };

        summary.unlocked = save.hasEnding(this.id, $elem.text());

        /* Check what conditions we can for this epilogue: */
        summary.gender = $elem.attr('gender') || 'any';
        if (summary.gender !== humanPlayer.gender && summary.gender !== 'any') {
            summary.wrongGender = true;
        }

        var alsoPlaying = $elem.attr("alsoPlaying");
        if (alsoPlaying) {
            alsoPlaying = alsoPlaying.trim().split(/\s+/);
            if (!alsoPlaying.every(function(ap) {
                return players.some(function (p) { return p.id == ap; });
            })) {
                /* Player requirement not met */
                summary.characterIsMissing = true;
            }
            summary.requiredCharacters = alsoPlaying;
            summary.requiredCharactersLabels = alsoPlaying.map(function(id) {
                var opp = loadedOpponents.find(function (p) {
                    return p && p.id === id;
                });
                return opp ? opp.selectLabel : id.initCap();
            });
        }

        summary.hint = $elem.attr('hint');
        summary.description = $elem.attr('description');
        summary.extraConditions = $elem.attr('markers') == 'true';
        summary.score = (summary.wrongGender ? 4 : 0)
            + (summary.characterIsMissing ? 2 : 0)
            + (summary.requiredCharacters || summary.extraConditions ? 1 : 0);

        ret.push(summary);
    }.bind(this));

    return ret;
}

/**
 * Helper function that trims down opponent status into something that'll be
 * easy to display on a selection screen.
 *
 * To be specific, this attempts to identify the epilogue with the most matching
 * static requirements, taking the player's gender and current table setup into
 * account.
 */
Opponent.prototype.getEpilogueStatus = function(mainSelect) {
    /* Find the epilogue that matches the most requirements possible.
     * Prefer matching gender requirements first before character reqs.
     */
    if (!this.endings) {
        return;
    }

    var epilogueStatus = this.getAllEpilogueStatus();
    var epilogueTitles = new Set();
    var epiloguesUnlocked = new Set();
    var bestMatchEpilogue = null;
    for (var i = 0; i < epilogueStatus.length; i++) {
        var status = epilogueStatus[i];
        epilogueTitles.add(status.title);
        if (status.unlocked) {
            epiloguesUnlocked.add(status.title);
            continue;
        }

        if (!bestMatchEpilogue || status.score < bestMatchEpilogue.score) {
            bestMatchEpilogue = status;
        }
    }

    /* Prior to main selection screen, show gender icon for wrong
     * gender and conditional icon for other conditions, fulfilled or
     * not (on the group selection screen, we'd need to look at the
     * other characters in the group rather than the currently
     * selected ones to give an accurate statement; let's do that
     * later).  On the main selection screen, show the warning icon
     * for wrong gender or character missing and the conditional icon
     * for marker conditions.
     */
    var badge = '';
    if (epiloguesUnlocked.size == epilogueTitles.size) {
        badge = "-completed";
    } else if (mainSelect && bestMatchEpilogue.score > 1) {
        badge = "-unavailable";
    } else if (bestMatchEpilogue.wrongGender) {
        badge = '-' + bestMatchEpilogue.gender;
    } else if ((bestMatchEpilogue.requiredCharacters && !mainSelect) || bestMatchEpilogue.extraConditions) {
        badge = "-conditional";
    }
    var tooltip;
    if (bestMatchEpilogue) {
        if (bestMatchEpilogue.wrongGender) {
            tooltip = "Play as " + bestMatchEpilogue.gender + " for a chance to unlock another epilogue";
        } else if (bestMatchEpilogue.requiredCharacters && (!mainSelect || bestMatchEpilogue.characterIsMissing)) {
            bestMatchEpilogue.requiredCharactersAsText = englishJoin(bestMatchEpilogue.requiredCharacters.map(function(id) {
                var opp = loadedOpponents.find(function (p) {
                    return p && p.id === id;
                });
                return opp ? opp.selectLabel : id.initCap();
            }));

            tooltip = "Play with " + bestMatchEpilogue.requiredCharactersAsText
                + " for a chance to unlock another epilogue";
        } else if (bestMatchEpilogue.extraConditions) {
            if (bestMatchEpilogue.hint) {
                tooltip = "Hint: " + bestMatchEpilogue.hint;
            } else {
                tooltip = "Unknown conditions apply";
            }
        }
    }

    return {
        total: epilogueTitles.size,
        unlocked: epiloguesUnlocked.size,
        match: bestMatchEpilogue,
        badge: 'img/epilogue' + badge + '.svg',
        tooltip: tooltip,
    };
}

/* Called prior to removing a character from the table. */
Opponent.prototype.unloadOpponent = function () {
    Sentry.addBreadcrumb({
        category: 'select',
        message: 'Unloading opponent ' + this.id,
        level: 'info'
    });

    this.unloadStylesheet();
    updateAllBehaviours(this.slot, null, [[OPPONENT_DESELECTED]]);

    this.slot = undefined;
    this.selectInfo = null;
}

Opponent.prototype.fetchBehavior = function() {
    // Optionally, replace with fetchCompressedURL(this.folder + "behaviour.xml")
    return fetchXML(this.folder + "behaviour.xml").then(function($xml) {
        /* Always parse the stylesheet element, so we can use it both
         * when selecting a character (loading the entire behaviour)
         * and when playing an epilogue from the gallery */
        this.stylesheet = null;
        var stylesheet = $xml.children('stylesheet').text();
        if (stylesheet) {
            var m = stylesheet.match(/[a-zA-Z0-9()~!*:@,;\-.\/]+\.css/i);
            if (m) {
                this.stylesheet = 'opponents/'+this.id+'/'+m[0];
            }
        }
        return $xml;
    }.bind(this));
}

/**
 * Loads and parses the start of the behaviour XML file of the
 * given opponent.
 *
 * @returns {Promise<void>} A Promise that resolves after all loading is complete.
 * This includes calls to loadAlternateCostume() and onSelected().
 */
Opponent.prototype.loadBehaviour = function (slot, individual, selectInfo) {
    this.slot = slot;
    this.selectInfo = selectInfo;

    if (this.isLoaded()) {
        var p = null;
        
        if (this.selected_costume) {
            p = this.loadAlternateCostume();
        } else {
            this.unloadAlternateCostume();
            p = immediatePromise();
        }

        return p.then(function () {
            this.onSelected(individual);
        }.bind(this)).catch(function(err) {
            /* Handle any errors that loadAlternateCostume might throw. */
            console.error("Failed to load " + this.id);
            captureError(err);

            delete players[this.slot];
            updateSelectionVisuals();
        }.bind(this));
    }

    // start loading collectibles in parallel with behaviour.xml
    var collectiblesPromise = this.fetchCollectibles();

    /* Success callback.
     * 'this' is bound to the Opponent object.
     */
    return this.fetchBehavior()
        .then(function($xml) {
            Sentry.addBreadcrumb({
                category: 'select',
                message: 'Fetched and parsed opponent ' + this.id + ', initializing...',
                level: 'info'
            });

            this.xml = $xml;
            this.intelligences = $xml.children('intelligence');

            this.settings = $xml.find("behaviour>settings").map(function (index, elem) {
                return CharacterSettingsGroup.parseXML(this, $(elem));
            }.bind(this)).get();

            this.default_costume = {
                id: null,
                labels: $xml.children('label'),
                tags: this.originalTags,
                folders: this.folder,
                wardrobe: $xml.children('wardrobe'),
                gender: $xml.children('gender').text(),
                size: $xml.children('size').text(),
            };

            var poses = $xml.children('poses');
            var poseDefs = {};
            $(poses).children('pose').each(function (i, elem) {
                var def = new PoseDefinition($(elem), this);
                poseDefs[def.id] = def;
            }.bind(this));

            var setElems = $xml.children('pose-sets');
            var poseSets = {};
            $(setElems).children("set").each(
                (i, elem) => {
                    let parsed = PoseSet.parseXML(this, $(elem));
                    poseSets[parsed.id] = parsed;
                }
            );

            this.default_costume.poses = poseDefs;
            this.default_costume.poseSets = poseSets;

            /* Load forward-declarations for persistent markers. */
            $xml.find('persistent-markers>marker').each(function (i, elem) {
                this.persistentMarkers[$(elem).text()] = true;
            }.bind(this));

            this.targetedLines = {};

            /* Clone cases with alternative conditions/test, keeping
             * one alternative set of conditions and tests on the case
             * level of each case clone. This may create multiple
             * cases with the same oneShotId, which is what we want,
             * because the case clones should still be seen as the
             * same case.
             *
             * This means that the conditions on the case element as
             * well as any condition and test elements outside of
             * alternatives must always be fulfilled, along with all
             * the conditions of tests inside any of the alternative
             * elements. */
            $xml.children('behaviour').find('case>alternative:first-of-type').each(function() {
                var $case = $(this).parent();
                $case.children('alternative').each(function() {
                    // Make clone and insert after original case
                    var $clone = $case.clone().insertAfter($case);
                    // Remove all <alternative> elements from clone, leaving base conditions
                    $clone.children('alternative').remove();
                    // Append conditions from this alternative to cloned case
                    $clone.append($(this).children());
                    for (var i = 0; i < this.attributes.length; i++) {
                        $clone.attr(this.attributes[i].name, this.attributes[i].value);
                    }
                });
                $case.remove();
            });

            var nicknames = {};
            $xml.children('nicknames').children('nickname').each(function() {
                if ($(this).attr('for') in nicknames) {
                    nicknames[$(this).attr('for')].push($(this).text());
                } else {
                    nicknames[$(this).attr('for')] = [ $(this).text() ];
                }
            });
            this.nicknames = nicknames;

            return this.loadXMLTriggers();
        }.bind(this)).then(function () {
            /* Wait for loading of all other stuff to complete: */
            if (this.selected_costume) {
                return Promise.all([this.loadAlternateCostume(), collectiblesPromise]);
            }

            return collectiblesPromise;
        }.bind(this)).then(
            this.onSelected.bind(this, individual)
        ).catch(function(err) {
            /* Error callback. */
            console.error("Failed to load " + this.id);
            captureError(err);
            
            delete players[this.slot];
            updateSelectionVisuals();
        }.bind(this));
}

Opponent.prototype.recordTargetedCase = function (caseObj) {
    var entities = new Set();

    if (caseObj.target) entities.add(caseObj.target);
    if (caseObj.alsoPlaying) entities.add(caseObj.alsoPlaying);
    if (caseObj.filter && caseObj.filter[0] !== "!") entities.add(caseObj.filter);

    caseObj.counters.forEach(function (ctr) {
        /* Conditions checking if a character/tag is not at the table don't count as targeted. */
        if (ctr.id && ctr.count.max !== 0) entities.add(ctr.id);
        if (ctr.tag) {
            if (ctr.tag[0] !== "!" && ctr.count.max !== 0) {
                entities.add(ctr.tag);
            } else if (ctr.tag[0] === "!" && ctr.count.max === 0) {
                /* (filter="!tag" and count: 0) implies checking if everyone has the given tag */
                entities.add(ctr.tag.substring(1));
            }
        }
    });

    var lines = new Set();
    caseObj.states.forEach(function (s) {
        lines.add(s.rawDialogue);

        /* Handle the old persist-marker flag by adding all markers set with
         * persist-marker="true" to the persistentMarkers list.
         *
         * TODO: Remove this once all characters using persistent markers
         * have migrated over to the system in #74.
         */
        if (s.legacyPersistentFlag) {
            s.markers.forEach(function (marker) {
                this.persistentMarkers[marker.name] = true;
            }.bind(this));
        }
    }.bind(this));

    entities.forEach(function (ent) {
        if (!(ent in this.targetedLines)) {
            this.targetedLines[ent] = { count: 0, seen: new Set() };
        }

        lines.forEach(Set.prototype.add, this.targetedLines[ent].seen);
    }, this);
}

/**
 * Traverses a new-format opponent's behaviour <trigger> elements
 * and pre-emptively adds their Cases to the opponent's cases structure.
 * This is done in 50ms chunks to avoid blocking the UI.
 *
 * @returns {Promise<number>} A Promise that resolves once all cases have been processed.
 */
Opponent.prototype.loadXMLTriggers = function () {
    return new Promise(function (resolve) {
        var $cases = this.xml.find('>behaviour>trigger>case:not([disabled="1"])');

        var loadItemsTotal = $cases.length;
        if (loadItemsTotal == 0) {
            return resolve(0);
        }

        var loadItemsCompleted = 0;
        function process() {
            var startTS = performance.now();

            /* break tasks into roughly 50ms chunks */
            while (performance.now() - startTS < 50) {
                if (loadItemsCompleted >= loadItemsTotal) {
                    this.loadProgress = undefined;
                    return resolve(loadItemsCompleted);
                }

                let $case = $($cases.get(loadItemsCompleted));
                let trigger = $case.parent().attr('id');
                let c = new Case($case, trigger);
                this.recordTargetedCase(c);

                c.getStages().forEach(function (stage) {
                    var key = c.trigger+':'+stage;  // Case constructor may have altered the trigger
                    if (!this.cases.has(key)) {
                        this.cases.set(key, []);
                    }

                    this.cases.get(key).push(c);
                }, this);

                loadItemsCompleted++;
            }

            this.loadProgress = loadItemsCompleted / loadItemsTotal;
            mainSelectDisplays[this.slot - 1]?.updateLoadPercentage(this);

            setTimeout(process.bind(this), 10);
        }

        setTimeout(process.bind(this), 0);
    }.bind(this));
}

Player.prototype.getImagesForStage = function (stage) {
    if(!this.xml) return [];

    var poseSet = {};
    var imageSet = {};
    
    function processCase (c) {
        /* Skip cases requiring characters that aren't present. */
        if (c.target && !players.some(function (p) { return p.id === c.target; })) return;
        if (c.alsoPlaying && !players.some(function (p) { return p.id === c.alsoPlaying; })) return;
        if (c.filter && !players.some(function (p) { return p.hasTag(c.filter); })) return;

        if (!c.counters.every(function (ctr) {
            var count = players.countTrue(function(p) {
                if (ctr.id && p.id !== ctr.id) return false;
                if (ctr.tag && !p.hasTag(ctr.tag)) return false;

                return true;
            });

            return inInterval(count, ctr.count);
        })) return;

        /* Collate pose names into poseSet. */
        c.getPossibleImages(stage === -1 ? 0 : stage).forEach(function (poseName) {
            poseSet[poseName] = true;
        });
    }

    if (stage > -1) {
        /* Find all cases that can play within this stage, then process
         * them.
         */

        var keySuffix = ':'+stage;
        this.cases.forEach(function (caseList, key) {
            if (!key.endsWith(keySuffix)) return;
            caseList.forEach(processCase);
        });
    } else {
        if (this.cases.has(GAME_START + ':0')) {
            this.cases.get(GAME_START + ':0').forEach(processCase);
        }
    }

    /* Finally, transform the set of collected pose names into a
     * set of image file paths.
     */
    Object.keys(poseSet).forEach((poseName) => {
        var actualStage = (stage > -1) ? stage : 0;
        var resolved = this.resolvePoseName(poseName, actualStage);

        if (!resolved) return;
        if (resolved instanceof PoseSet || resolved instanceof PoseDefinition) {
            resolved.getUsedImages(actualStage).forEach((img) => {
                imageSet[img] = true;
            });
        } else {
            imageSet[resolved] = true;
        }
    });

    return Object.keys(imageSet);
};

/**
 * Preload all images referenced by this character's dialogue for a given stage.
 * @param {number} stage 
 * @returns {Promise<Array<HTMLImageElement>>}
 */
Player.prototype.preloadStageImages = function (stage) {
    return Promise.all(this.getImagesForStage(stage).map(function (fn) {
        return new Promise(function (resolve, reject) {
            /* Keep references to the Image elements around so they don't get GC'd. */
            if (this.imageCache[fn]) {
                resolve(this.imageCache[fn]);
            } else {
                var img = new Image();
                img.addEventListener('load', function() { resolve(img); });
                img.src = fn;
                this.imageCache[fn] = img;
            }
        }.bind(this));
    }, this));
};

/**
 * 
 * @param {Player} player
 * @param {string} marker 
 * @param {CharacterSetting[]} settings 
 */
function CharacterSettingsGroup (player, marker, settings) {
    this.marker = marker;
    this.player = player;
    this.settings = settings; /* preserve order for dropdown display */
    this.defaultSetting = settings.find((value) => value.isDefault) || null;
}

CharacterSettingsGroup.parseXML = function (player, $xml) {
    var marker = $xml.attr("marker");
    var settings = $xml.children("setting").map(function (index, elem) {
        return CharacterSetting.parseXML(player, $(elem));
    }).get();

    return new CharacterSettingsGroup(player, marker, settings);
}

CharacterSettingsGroup.prototype.update = function () {
    var markerVal = this.player.getMarker(this.marker);
    var setTo = this.settings.find((setting) => (setting.value == markerVal) && setting.isAvailable()) || this.defaultSetting;
    this.setSelected(setTo ? setTo.value : "");
}

CharacterSettingsGroup.prototype.reset = function () {
    if (this.player.persistentMarkers[this.marker]) {
        this.update();
    } else {
        this.setSelected(this.defaultSetting ? this.defaultSetting.value : "");
    }
}

CharacterSettingsGroup.prototype.setSelected = function (value) {
    this.player.setMarker(this.marker, null, value || "");
}

CharacterSettingsGroup.prototype.getSelected = function () {
    var markerVal = this.player.getMarker(this.marker);
    var setting = this.settings.find((setting) => (setting.value == markerVal) && setting.isAvailable());
    if (setting && !setting.isAvailable()) {
        setting = this.defaultSetting;
    }

    return setting || this.defaultSetting || null;
}

CharacterSettingsGroup.prototype.getAvailable = function () {
    return this.settings.filter((setting) => setting.isAvailable());
}

Player.prototype.populateDebugMarkers = function () {
    /** @type {{[baseName: string]: {[oppId: string]: string | number}}} */
    var resolvedMarkers = {};
    var rawPersistentMarkers = save.getAllPersistentMarkers(this);
    var unresolvedNames = new Set();

    for (let key of Object.keys(this.markers ?? {})) {
        unresolvedNames.add(key);
    }

    for (let key of Object.keys(rawPersistentMarkers)) {
        unresolvedNames.add(key);
    }

    /* Match longer IDs before shorter ones, to account for opponents with IDs that are prefixes of each other.
     * Also make sure to check for markers targeted towards the human player.
     */
    let allPlayers = loadedOpponents.slice();
    allPlayers.push(players[0]);

    for (let opp of allPlayers.sort((a, b) => b.id.length - a.id.length)) {
        let markerPrefix = "__" + opp.id + "_";
        let matchingMarkers = Array.from(unresolvedNames.entries()).map((entry) => entry[0]).filter((name) => name.startsWith(markerPrefix));

        for (let name of matchingMarkers) {
            let baseName = name.substring(markerPrefix.length);
            let value = this.persistentMarkers[baseName] ? rawPersistentMarkers[name] : this.markers[name];
            value ??= this.markers[name];

            if (!resolvedMarkers[baseName]) resolvedMarkers[baseName] = {};
            resolvedMarkers[baseName][opp.id] = value;

            unresolvedNames.delete(name);
        }
    }

    /* All remaining unresolved names are probably non-targeted markers. */
    for (let name of unresolvedNames) {
        let value = this.persistentMarkers[name] ? rawPersistentMarkers[name] : this.markers[name];
        value ??= this.markers[name];

        if (!resolvedMarkers[name]) resolvedMarkers[name] = {};
        resolvedMarkers[name][""] = value;
    }


    var foundPersistentMarkers = false;
    var foundRegularMarkers = false;

    $("#debug-marker-listing-persistent").empty();
    $("#debug-marker-listing-regular").empty();

    /* Sort entries by base name. */
    for (let basePair of Object.entries(resolvedMarkers).sort(
        (a, b) => a[0] > b[0]
    )) {
        let baseName = basePair[0];

        for (let idPair of Object.entries(basePair[1])) {
            let container = $('<div>', {"class": "debug-marker-entry"});
            container.append($('<span>', {"class": "debug-marker-basename", "text": baseName}));
            
            if (idPair[0] !== "") {
                container.append($('<span>', {
                    "class": "debug-marker-target-id",
                    "text": idPair[0]
                }));
            }

            if (idPair[1] === "") {
                container.append($('<span>', {
                    "class": "debug-marker-value empty-value",
                    "text": "~blank~"
                }));
            } else if (!isNaN(parseInt(idPair[1], 10))) {
                container.append($('<span>', {
                    "class": "debug-marker-value",
                    "text": idPair[1].toString()
                }));
            } else {
                container.append($('<span>', {
                    "class": "debug-marker-value",
                    "text": '\"' + idPair[1].toString() + '\"'
                }));
            }

            if (this.persistentMarkers[baseName]) {
                $("#debug-marker-listing-persistent").append(container);
                foundPersistentMarkers = true;
            } else {
                $("#debug-marker-listing-regular").append(container);
                foundRegularMarkers = true;
            }
        }
    }

    if (foundPersistentMarkers) {
        $("#debug-marker-column-persistent").show();
    } else {
        $("#debug-marker-column-persistent").hide();
    }

    if (foundRegularMarkers) {
        $("#debug-marker-column-regular").show();
    } else {
        $("#debug-marker-column-regular").hide();
    }

    var statusIndicator = $("#debug-marker-header .debug-info-header-status");
    var header = $("#debug-marker-header");
    var container = $("#debug-marker-container");
    
    if (foundPersistentMarkers || foundRegularMarkers) {
        container.show();
        header.show();

        let collapsed = false;
        header.on("click", function () {
            collapsed = !collapsed;
            if (collapsed) {
                statusIndicator.removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-up");
                container.slideUp();
            } else {
                statusIndicator.removeClass("glyphicon-chevron-up").addClass("glyphicon-chevron-down");
                container.slideDown();
            }
        })
    } else {
        container.hide();
        header.hide();
        header.off("click");
    }
}

Player.prototype.populateDebugTags = function () {
    $("#debug-tag-listing").empty();

    for (let tag of this.tags) {
        if (tag === this.id) continue;
        if (this.alt_costume && tag === this.alt_costume.id) continue;

        var entry = $('<div>', {"class": "debug-tag-entry"});
        entry.append($('<span>', {"class": "debug-tag-name", "text": tag}));
        
        if (this.tagModifications[tag] && this.tagModifications[tag].present) {
            entry.append($('<span>', {"class": "debug-tag-modified-stage", "text": "added in stage " + this.tagModifications[tag].stage}))
                .prependTo("#debug-tag-listing");
        } else {
            entry.appendTo("#debug-tag-listing");
        }
    }

    for (let tag of Object.keys(this.tagModifications)) {
        if (!this.tagModifications[tag].present && !this.tags.includes(tag)) {
            $('<div>', {"class": "debug-tag-entry"})
                .append($('<span>', {"class": "debug-tag-name debug-tag-removed", "text": tag}))
                .append($('<span>', {"class": "debug-tag-modified-stage", "text": "removed in stage " + this.tagModifications[tag].stage}))
                .prependTo("#debug-tag-listing");
        }
    }

    var statusIndicator = $("#debug-tag-header .debug-info-header-status");
    var header = $("#debug-tag-header");
    var container = $("#debug-tag-container");
    
    let collapsed = false;
    header.on("click", function () {
        collapsed = !collapsed;
        if (collapsed) {
            statusIndicator.removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-up");
            container.slideUp();
        } else {
            statusIndicator.removeClass("glyphicon-chevron-up").addClass("glyphicon-chevron-down");
            container.slideDown();
        }
    });
}

/**
 * 
 * @param {Player} player
 * @param {string} value 
 * @param {string} name
 * @param {boolean} isDefault
 * @param {VariableTest[]} tests 
 */
function CharacterSetting (player, value, name, isDefault, tests) {
    this.player = player;
    this.value = value;
    this.name = name || value;
    this.isDefault = isDefault;
    this.tests = tests;
}

CharacterSetting.parseXML = function (player, $xml) {
    var tests = $xml.children("test").map(function (index, elem) {
        return VariableTest.parseXML($(elem));
    }).get();

    var isDefault = ($xml.attr("default") || "false") == "true";
    var value = $xml.attr("value") || "";
    var name = $xml.children("name").text();

    return new CharacterSetting(player, value, name, isDefault, tests);
}

CharacterSetting.prototype.isAvailable = function () {
    return this.tests.every((test) => test.evaluate(this.player, null, null));
}

/**
 * 
 * @param {Condition} condition
 * @returns {string} 
 */
function formatConditionInfo(condition) {
    let attributes = {
        id: "character",
        tag: "tag",
        nottag: "not tag",
        tagAdv: "tagAdv",
        stage: "stage",
        layers: "layers",
        startingLayers: "starting layers",
        gender: "gender",
        status: "status",
        timeInStage: "time in stage",
        hand: "has hand",
        consecutiveLosses: "consecutive losses",
        saidMarker: "said marker",
        sayingMarker: "saying marker",
        notSaidMarker: "not said marker",
        saying: "saying text",
        said: "said text",
        pose: "pose",
        count: "count"
    };

    let attributeElems = Object.entries(attributes).filter(
        (pair) => condition[pair[0]] !== undefined && condition[pair[0]] !== null
    ).map(
        (pair) => {
            var value = condition[pair[0]];
            var formattedValue = "";
            if (value instanceof Interval) {
                let hasMin = value.min !== null && !isNaN(value.min);
                let hasMax = value.max !== null && !isNaN(value.max);
                if (hasMin && hasMax) {
                    formattedValue = value.toString();
                } else if (!hasMin && hasMax) {
                    formattedValue = "<= " + value.max;
                } else if (hasMin && !hasMax) {
                    formattedValue = value.min + "+";
                } else {
                    formattedValue = "any";
                }
            } else {
                formattedValue = value.toString();
            }

            return $("<span>", {"class": "debug-case-condition-attribute"}).append(
                $("<span>", {"class": "debug-case-condition-type", "text": pair[1]}),
                $("<span>", {"class": "debug-case-condition-value", "text": formattedValue})
            );
        }
    );

    var role = condition.role ?? "any";
    attributeElems.splice(0, 0, $("<span>", {"class": "debug-case-condition-role", "text": role.initCap()}));

    return attributeElems;
}

function createDebugSectionRow(header, value) {
    return $(
        "<div>", {"class": "debug-section-row"}
    ).append(
        $("<div>", {"class": "debug-section-header", "text": header}),
        $("<div>", {"class": "debug-section-value"}).append(value)
    );
}

Player.prototype.populateDebugCaseInfo = function () {
    if (!this.chosenState || !this.chosenState.parentCase) {
        $("#debug-case-info-container").hide();
        return;
    }

    var chosenCase = this.chosenState.parentCase;
    var caseTypeDisplay = [
        $("<span>", {"class": "debug-case-type", "text": chosenCase.trigger}),
        ", priority ",
        $("<span>", {
            "class": "debug-case-priority",
            "text": ((chosenCase.customPriority !== undefined) ? "*" : "") + chosenCase.priority.toString()
        })
    ];

    var caseStageDisplay = $(
        "<span>",
        {"class": "debug-case-stage", "text": chosenCase.stage.toString()}
    )

    var listing = $("#debug-case-info-listing");
    listing.empty().append(
        createDebugSectionRow("Type", caseTypeDisplay),
        createDebugSectionRow("Stages", caseStageDisplay),
        chosenCase.counters.map(
            (condition) => createDebugSectionRow("Condition", formatConditionInfo(condition))
        )
    );

    for (let test of chosenCase.tests) {
        let cmp = test.cmp || "==";
        let value = test.value || "";
        
        if (!isNaN(parseInt(value, 10))) {
            value = parseInt(value, 10).toString();
        } else {
            value = '\"' + value + '\"';
        }

        listing.append(
            createDebugSectionRow("Test", $("<span>", {"class": "debug-case-test", "text": test.expr + " " + cmp + " " + value}))
        );
    }

    var setsMarkers = this.chosenState.markers.map(
        (marker) => {
            let formatted = marker.name;
            if (marker.perTarget) formatted += "*";
            
            if (marker.op == "=") {
                formatted += " = ";
            } else {
                formatted += " " + marker.op + "= ";
            }

            formatted += marker.rhs;
            return formatted;
        }
    ).join(", ");

    if (setsMarkers) {
        createDebugSectionRow("Sets Markers", $(
            "<span>",
            {"class": "debug-case-markers", "text": setsMarkers}
        )).appendTo(listing);
    }

    var formattedPose = this.chosenState.image ? (
        this.chosenState.image.replace(/\.(?:jpe?g|png|gif)$/i, "").replace("#", this.stage)
    ) : "<none>";

    createDebugSectionRow("Pose", $(
        "<span>",
        {"class": "debug-case-pose", "text": formattedPose}
    )).appendTo(listing);

    var header = $("#debug-case-info-header");
    var container = $("#debug-case-info-container");
    var statusIndicator = $("#debug-case-info-header .debug-info-header-status");
    
    let collapsed = false;
    header.on("click", function () {
        collapsed = !collapsed;
        if (collapsed) {
            statusIndicator.removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-up");
            container.slideUp();
        } else {
            statusIndicator.removeClass("glyphicon-chevron-up").addClass("glyphicon-chevron-down");
            container.slideDown();
        }
    }).show();
    container.show();
}

Player.prototype.populateDebugStatusInfo = function () {
    /* Statuses not listed here can be easily inferred. */
    var clothingStatusNames = {
        "mostly_clothed": "lost only accessories",
        "decent": "still covered by major articles",
        "chest_visible": "chest visible",
        "crotch_visible": "crotch visible",
        "topless": "topless (not naked)",
        "bottomless": "bottomless (not naked)",
        "naked": "naked (fully exposed)",
    };

    var stageName = "";
    if (this.stage == 0) {
        stageName = "Fully Clothed";
    } else if (this.finished) {
        stageName = "Finished";
    } else if (this.out) {
        stageName = "Masturbating";
    } else {
        stageName = "Lost " + this.removedClothing.name.initCap();
    }

    var applicableStatusFlags = Object.entries(clothingStatusNames).filter(
        (pair) => this.checkStatus(pair[0])
    ).map(
        (pair) => pair[1]
    ).join(", ");

    var listing = $("#debug-character-status-listing");
    listing.empty().append(
        createDebugSectionRow("Stage", this.stage + " (" + stageName + ")"),
        createDebugSectionRow("Time in Stage", this.timeInStage + " (" + this.ticksInStage + " ticks)"),
        createDebugSectionRow("AI", this.intelligence.initCap()),
        createDebugSectionRow("Biggest Lead", this.biggestLead)
    );

    if (applicableStatusFlags) {
        createDebugSectionRow("Clothing Status", applicableStatusFlags.initCap()).appendTo(listing);
    }

    if (this.out) {
        createDebugSectionRow("Out Order", this.outOrder).appendTo(listing);
        
        if (this.finishingTarget && this.finishingTarget !== this) {
            createDebugSectionRow("Finish Redirect Target", this.finishingTarget.id + " (slot " + this.finishingTarget.slot + ")").appendTo(listing);
        }
    }

    if (this.out && !this.finished) {
        createDebugSectionRow(
            "Timer", this.timer + (this.forfeit[0] === PLAYER_HEAVY_MASTURBATING ? " (heavy)" : "")
        ).appendTo(listing);
    } else if (!this.out) {
        createDebugSectionRow("Stamina", this.stamina).appendTo(listing);
        createDebugSectionRow("Consec. Losses", this.consecutiveLosses).appendTo(listing);
    }

    var header = $("#debug-character-status-header");
    var container = $("#debug-character-status-container");
    var statusIndicator = $("#debug-character-status-header .debug-info-header-status");
    
    let collapsed = false;
    header.on("click", function () {
        collapsed = !collapsed;
        if (collapsed) {
            statusIndicator.removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-up");
            container.slideUp();
        } else {
            statusIndicator.removeClass("glyphicon-chevron-up").addClass("glyphicon-chevron-down");
            container.slideDown();
        }
    }).show();
    container.show();
}

Player.prototype.showDebugModal = function() {
    $("#debug-info-current-character").text(this.id.replace(/_/g, ' ').initCap());

    this.populateDebugStatusInfo();
    this.populateDebugCaseInfo();
    this.populateDebugMarkers();
    this.populateDebugTags();

    $characterDebugModal.modal("show");
}

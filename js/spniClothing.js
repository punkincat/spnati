/********************************************************************************
 This file contains the variables and functions that store information on player
 clothing and player stripping.
 ********************************************************************************/

/**********************************************************************
 *****                Clothing Object Specification               *****
 **********************************************************************/

/* clothing types */
var IMPORTANT_ARTICLE = "important";
var MAJOR_ARTICLE = "major";
var MINOR_ARTICLE = "minor";
var EXTRA_ARTICLE = "extra";

/* clothing positions */
var UPPER_ARTICLE = "upper";
var LOWER_ARTICLE = "lower";
var FULL_ARTICLE = "both";
var OTHER_ARTICLE = "other";

var STATUS_LOST_SOME = "lost_some"
var STATUS_MOSTLY_CLOTHED = "mostly_clothed"
var STATUS_DECENT = "decent";
var STATUS_EXPOSED = "exposed";
var STATUS_EXPOSED_TOP = "chest_visible";
var STATUS_EXPOSED_BOTTOM = "crotch_visible";
var STATUS_EXPOSED_TOP_ONLY = "topless";
var STATUS_EXPOSED_BOTTOM_ONLY = "bottomless";
var STATUS_NAKED = "naked";
var STATUS_LOST_ALL = "lost_all";
var STATUS_ALIVE = "alive";
var STATUS_MASTURBATING = "masturbating";
var STATUS_HEAVY_MASTURBATING = "heavy_masturbating";
var STATUS_FINISHED = "finished";

/************************************************************
 * Stores information on an article of clothing.
 ************************************************************/
function Clothing (name, generic, type, position, plural, fromStage, fromDeal, reveal, strippingLayer) {
    if (name instanceof jQuery) {
        const $xml = name;
        generic = $xml.attr('generic');
        name = $xml.attr('name');
        type = $xml.attr('type');
        position = $xml.attr('position');
        plural = $xml.attr('plural');
        plural = (plural == 'null' ? null : plural == 'true');
        fromStage = Number.parseInt($xml.attr('fromStage'), 10) || undefined; // fromStage=0 is the default anyway
        fromDeal = $xml.attr('fromDeal') === 'true';
        reveal = $xml.attr('reveal');
        if ($xml.children('stripping').length) {
            this.strippingItem = new Clothing($xml.children('stripping').first());
        } else if ($xml.attr('strippingLayer') !== undefined) {
            strippingLayer = Number.parseInt($xml.attr('strippingLayer'), 10);
            if (isNaN(strippingLayer) || strippingLayer >= $xml.index() || strippingLayer < 0) {
                console.error('Invalid stripping layer');
                strippingLayer = undefined;
            }
        }
    }
    this.name = name;
    this.generic = generic || name;
    this.type = type;
    this.position = position;
    this.plural = (plural === undefined ? false : plural);
    this.fromStage = fromStage;
    this.fromDeal = fromDeal;
    this.reveal = reveal;
    this.strippingLayer = strippingLayer;
    this.removed = false;
}

/*************************************************************
 * Check if the player has major articles covering both the upper and
 * lower body.  Currently only used to determine whether the human
 * player is "decent".
 *************************************************************/
Player.prototype.isDecent = function() {
    return this.getClothing().some(function(c) {
        return (c.position == UPPER_ARTICLE || c.position == FULL_ARTICLE) && c.type == MAJOR_ARTICLE;
    }) && this.getClothing().some(function(c) {
        return (c.position == LOWER_ARTICLE || c.position == FULL_ARTICLE) && c.type == MAJOR_ARTICLE;
    });
};

/*************************************************************
 * Check if the player chest and/or crotch is covered (not exposed).
 * If except is defined, that article is ignored (because it's being stripped)
 *************************************************************/
Player.prototype.isCovered = function(position, stageDelta, removedClothing) {
    if (position == FULL_ARTICLE || position === undefined) {
        return this.isCovered(UPPER_ARTICLE, stageDelta, removedClothing)
            && this.isCovered(LOWER_ARTICLE, stageDelta, removedClothing);
    }
    return this.getClothing(stageDelta, removedClothing).some(function(c) {
        return (c.type == IMPORTANT_ARTICLE || c.type == MAJOR_ARTICLE)
            && (c.position == position || c.position == FULL_ARTICLE);
    });
};

/**************************************************************
 * Look through player's remaining clothing for items of certain
 * types, in certain positions, and with certain names, excluding ones
 * covered by others. 
 **************************************************************/
Player.prototype.findClothing = function(types, positions, names) {
    return this.getClothing().filter((c, i, clothing) =>
        (types === undefined || types.includes(c.type))
            && (positions === undefined || positions.includes(c.position))
            && (names === undefined || names.includes(c.name)
                || names.includes(c.generic))
        // Check if the article can be seen
            && !clothing.some((c2, j) =>
                /* "both" covers both "upper" and "lower" */
                (c2.position == c.position
                 || c2.position == FULL_ARTICLE && [UPPER_ARTICLE, LOWER_ARTICLE].includes(c.position))
                /* "extra" (or other future types never covers anything) */
                    && [MINOR_ARTICLE, MAJOR_ARTICLE, IMPORTANT_ARTICLE].includes(c2.type)
                /* "important" is assumed to actually be beneath all major articles */
                    && (j > i || (c.type == IMPORTANT_ARTICLE && c2.type == MAJOR_ARTICLE))
                    && !(c.type == MAJOR_ARTICLE && c2.type == IMPORTANT_ARTICLE)
            )
    );
};

/**
 * 
 * @param {string} name The specific name of the clothing article.
 * @param {string} generic The generic name for the clothing article.
 * @param {string} type The clothing's type.
 * @param {string} position The clothing's position.
 * @param {string} image The image used for the clothing in selection menus.
 * @param {Boolean} plural Whether or not the clothing is plural.
 * @param {string} id A unique ID for this clothing. If this clothing is attached to a collectible,
 * the ID of the character associated with that collectible will be prepended to this ID.
 * @param {string} applicable_genders What genders are allowed to wear this clothing. Can be "all".
 * @param {Collectible} collectible An optional collectible that must be unlocked before this clothing can be selected.
 */
function PlayerClothing (
    name, generic, type, position, image, plural, id, applicable_genders, collectible
) {
    Clothing.call(this, name, generic, type, position, plural);

    this.id = ((collectible && collectible.player) ? "" : "_default.") + id;
    this.image = image;
    this.applicable_genders = applicable_genders.toLowerCase();
    this.collectible = collectible;
}

PlayerClothing.prototype = Object.create(Clothing.prototype);
PlayerClothing.prototype.constructor = PlayerClothing;

/**
 * Get whether this clothing is available to wear at the title screen.
 * @returns {Boolean}
 */
PlayerClothing.prototype.isAvailable = function () {
    if (this.applicable_genders !== "all" && humanPlayer.gender !== this.applicable_genders) {
        return false;
    }

    return !this.collectible || this.collectible.isUnlocked();
}

/**
 * Create an HTML <button> element for this clothing.
 * @returns {HTMLButtonElement}
 */
PlayerClothing.prototype.createSelectionElement = function () {
    var img = document.createElement("img");
    img.setAttribute("src", this.image);
    img.setAttribute("alt", this.name.initCap());
    img.className = "custom-clothing-img";

    var elem = document.createElement("button");
    elem.className = "bordered player-clothing-select";
    elem.appendChild(img);

    return elem;
}

PlayerClothing.prototype.tooltip = function () {
    if (!this.collectible) return this.name.initCap();

    if (this.isAvailable()) {
        let tooltip = this.collectible.title;
        if (this.collectible.player && tooltip.indexOf(this.collectible.player.metaLabel) < 0) {
            tooltip += " - from " + this.collectible.player.metaLabel;
        }
        return tooltip;
    } else {
        return "To unlock: " + this.collectible.unlock_hint;
    }
};

/**
 * Get whether this clothing has been selected for the current player gender.
 * @returns {Boolean}
 */
PlayerClothing.prototype.isSelected = function () {
    return save.isClothingSelected(this);
}

/**
 * Set whether this clothing has been selected for the current player gender.
 * @param {Boolean} selected 
 */
PlayerClothing.prototype.setSelected = function (selected) {
    return save.setClothingSelected(this, selected);
}

/**********************************************************************
 *****                    Stripping Variables                     *****
 **********************************************************************/

/* stripping modal */
$stripModal = $("#stripping-modal");
$stripClothing = $("#stripping-clothing-area");
$stripButton = $("#stripping-modal-button");

/**
 * @type {StripClothingSelectionIcon[]}
 */
var clothingStripSelectors = [];

/**********************************************************************
 *****                      Strip Functions                       *****
 **********************************************************************/

 /**
  * Calculate the position a removed article of clothing reveals, if any.
  *
  * This is either FULL_ARTICLE, LOWER_ARTICLE, UPPER_ARTICLE, or null.
  *
  * @param {Player} player
  * @param {Clothing} clothing
  * @returns {string?} The revealed position, if any.
  */
function getRevealedPosition (player, clothing) {
    var type = clothing.type;
    var pos = clothing.position;

    /* Reveals only happen for important and major articles in the upper, lower or both positions.
     * This is just a short-circuit; the rest of the function will give the correct result without it. */
    if (![IMPORTANT_ARTICLE,  MAJOR_ARTICLE].includes(type)
        || ![UPPER_ARTICLE, LOWER_ARTICLE, FULL_ARTICLE].includes(pos)) {
        return null;
    }
    if (clothing.reveal !== undefined && !clothing.removed) {
        return clothing.reveal == "none" ? null : clothing.reveal;
    }

    const [hasLower, hasUpper] = [LOWER_ARTICLE, UPPER_ARTICLE].map(pos => player.isCovered(pos, -1));
    const [willHaveLower, willHaveUpper] = [LOWER_ARTICLE, UPPER_ARTICLE].map(pos => player.isCovered(pos, 1, clothing));

    if (hasUpper && hasLower && !willHaveUpper && !willHaveLower) {
            /* Article exposes both at once. */
        return FULL_ARTICLE;
    } else if (hasUpper && !willHaveUpper) {
        /* Article only exposes chest. */
        return UPPER_ARTICLE;
    } else if (hasLower && !willHaveLower) {
        /* Article only exposes crotch. */
        return LOWER_ARTICLE;
    }
    /* Article doesn't actually reveal anything. */
    return null;
}

 /************************************************************
 * Fetches the appropriate dialogue trigger for the provided
 * article of clothing, based on whether the article is going
 * to be removed or has been removed. Written to prevent duplication.
 ************************************************************/
function getClothingTrigger (player, clothing, removed) {
    var revealPos = getRevealedPosition(player, clothing);
    var type = clothing.type;
    var gender = player.gender;

    if (revealPos == UPPER_ARTICLE) {
        if (gender == eGender.MALE) {
            if (removed) {
                return [MALE_CHEST_IS_VISIBLE, OPPONENT_CHEST_IS_VISIBLE];
            } else {
                return [MALE_CHEST_WILL_BE_VISIBLE, OPPONENT_CHEST_WILL_BE_VISIBLE];
            }
        } else if (gender == eGender.FEMALE) {
            if (removed) {
                if (player.breasts == eSize.LARGE) {
                    return [FEMALE_LARGE_CHEST_IS_VISIBLE, FEMALE_CHEST_IS_VISIBLE, OPPONENT_CHEST_IS_VISIBLE];
                } else if (player.breasts == eSize.SMALL) {
                    return [FEMALE_SMALL_CHEST_IS_VISIBLE, FEMALE_CHEST_IS_VISIBLE, OPPONENT_CHEST_IS_VISIBLE];
                } else {
                    return [FEMALE_MEDIUM_CHEST_IS_VISIBLE, FEMALE_CHEST_IS_VISIBLE, OPPONENT_CHEST_IS_VISIBLE];
                }
            } else {
                return [FEMALE_CHEST_WILL_BE_VISIBLE, OPPONENT_CHEST_WILL_BE_VISIBLE];
            }
        }
    } else if ((revealPos == LOWER_ARTICLE) || (revealPos == FULL_ARTICLE)) {
        /* Treat full-article reveals as being crotch reveals for the purposes of case triggering. */
        if (gender == eGender.MALE) {
            if (removed) {
                if (player.penis == eSize.LARGE) {
                    return [MALE_LARGE_CROTCH_IS_VISIBLE, MALE_CROTCH_IS_VISIBLE, OPPONENT_CROTCH_IS_VISIBLE];
                } else if (player.penis == eSize.SMALL) {
                    return [MALE_SMALL_CROTCH_IS_VISIBLE, MALE_CROTCH_IS_VISIBLE, OPPONENT_CROTCH_IS_VISIBLE];
                } else {
                    return [MALE_MEDIUM_CROTCH_IS_VISIBLE, MALE_CROTCH_IS_VISIBLE, OPPONENT_CROTCH_IS_VISIBLE];
                }
            } else {
                return [MALE_CROTCH_WILL_BE_VISIBLE, OPPONENT_CROTCH_WILL_BE_VISIBLE];
            }
        } else if (gender == eGender.FEMALE) {
            if (removed) {
                if (player.penis === eSize.LARGE) {
                    return [FUTA_LARGE_CROTCH_IS_VISIBLE, FUTA_CROTCH_IS_VISIBLE, OPPONENT_CROTCH_IS_VISIBLE];
                } else if (player.penis === eSize.SMALL) {
                    return [FUTA_SMALL_CROTCH_IS_VISIBLE, FUTA_CROTCH_IS_VISIBLE, OPPONENT_CROTCH_IS_VISIBLE];
                } else if (player.penis === eSize.MEDIUM) {
                    return [FUTA_MEDIUM_CROTCH_IS_VISIBLE, FUTA_CROTCH_IS_VISIBLE, OPPONENT_CROTCH_IS_VISIBLE];
                } else {
                    return [FEMALE_CROTCH_IS_VISIBLE, OPPONENT_CROTCH_IS_VISIBLE];
                }
            } else {
                if (player.penis) {
                    return [FUTA_CROTCH_WILL_BE_VISIBLE, OPPONENT_CROTCH_WILL_BE_VISIBLE];
                } else {
                    return [FEMALE_CROTCH_WILL_BE_VISIBLE, OPPONENT_CROTCH_WILL_BE_VISIBLE];
                }
            }
        }
    } else {
        if (type == MAJOR_ARTICLE || type == IMPORTANT_ARTICLE) {
            if (gender == eGender.MALE) {
                return removed ? [MALE_REMOVED_MAJOR] : [MALE_REMOVING_MAJOR];
            } else if (gender == eGender.FEMALE) {
                return removed ? [FEMALE_REMOVED_MAJOR] : [FEMALE_REMOVING_MAJOR];
            }
        } else if (type == MINOR_ARTICLE) {
            if (gender == eGender.MALE) {
                return removed ? [MALE_REMOVED_MINOR] : [MALE_REMOVING_MINOR];
            } else if (gender == eGender.FEMALE) {
                return removed ? [FEMALE_REMOVED_MINOR] : [FEMALE_REMOVING_MINOR];
            }
        } else if (type == EXTRA_ARTICLE) {
            if (gender == eGender.MALE) {
                return removed ? [MALE_REMOVED_ACCESSORY] : [MALE_REMOVING_ACCESSORY];
            } else if (gender == eGender.FEMALE) {
                return removed ? [FEMALE_REMOVED_ACCESSORY] : [FEMALE_REMOVING_ACCESSORY];
            }
        }
    }

    /* Shouldn't get here... */
    console.error("Could not determine strip triggers for player ", player, " and clothing ", clothing);
    return [];
}

/************************************************************
 * Determines whether or not the provided player is winning
 * or losing and returns the appropriate dialogue trigger.
 ************************************************************/
function determineStrippingSituation (player) {
    /* determine if this player's clothing count is the highest or lowest */
    var isMax = true;
    var isMin = true;

    players.forEach(function(p) {
        if (p !== player) {
            if (p.countLayers() <= player.countLayers() - 1) {
                isMin = false;
            }
            if (p.countLayers() >= player.countLayers() - 1) {
                isMax = false;
            }
        }
    });

    /* return appropriate trigger */
    if (isMax) {
        return PLAYER_MUST_STRIP_WINNING;
    } else if (isMin) {
        return PLAYER_MUST_STRIP_LOSING;
    } else {
        return PLAYER_MUST_STRIP_NORMAL;
    }
}

/************************************************************
 * Manages the dialogue triggers before a player strips or forfeits.
 ************************************************************/
function playerMustStrip (player) {
    /* count the clothing the player has remaining */
    /* assume the player only has IMPORTANT_ARTICLES */
    var clothing = players[player].getClothing();

    saveTranscriptMessage("<b>"+players[recentLoser].label.escapeHTML()+"</b> has lost the hand"
                          + (players[player].countLayers() > 0 ? '.' : ', and is out of clothes.'));

    if (players[player].countLayers() > 0) {
        /* the player has clothes and will strip */
        if (player == HUMAN_PLAYER) {
            var trigger;
            if (clothing.length == 1 && (clothing[0].type == IMPORTANT_ARTICLE || clothing[0].type == MAJOR_ARTICLE)) {
                if (humanPlayer.gender == eGender.MALE) {
                    if (clothing[0].position == LOWER_ARTICLE) {
                        trigger = [[MALE_CROTCH_WILL_BE_VISIBLE, OPPONENT_CROTCH_WILL_BE_VISIBLE]];
                    } else {
                        trigger = [[MALE_CHEST_WILL_BE_VISIBLE, OPPONENT_CHEST_WILL_BE_VISIBLE]];
                    }
                } else {
                    if (clothing[0].position == LOWER_ARTICLE) {
                        trigger = [[FEMALE_CROTCH_WILL_BE_VISIBLE, OPPONENT_CROTCH_WILL_BE_VISIBLE]];
                    } else {
                        trigger = [[FEMALE_CHEST_WILL_BE_VISIBLE, OPPONENT_CHEST_WILL_BE_VISIBLE]];
                    }
                }
                humanPlayer.removedClothing = clothing[0];
            } else {
                if (humanPlayer.gender == eGender.MALE) {
                    trigger = [MALE_HUMAN_MUST_STRIP, MALE_MUST_STRIP, OPPONENT_LOST];
                } else {
                    trigger = [FEMALE_HUMAN_MUST_STRIP, FEMALE_MUST_STRIP, OPPONENT_LOST];
                }
            }
            
            updateAllBehaviours(player, null, trigger);
        } else {
            var trigger = determineStrippingSituation(players[player]);
            
            updateAllBehaviours(
                player,
                [trigger, PLAYER_MUST_STRIP],
                [[(players[player].gender == eGender.MALE ? MALE_MUST_STRIP : FEMALE_MUST_STRIP), OPPONENT_LOST]]
            );
        }
    } else {
        /* the player has no clothes and will have to accept a forfeit */
        var trigger = null;
        if (player != HUMAN_PLAYER) {
            trigger = determineForfeitSituation(player);
        }
        
        updateAllBehaviours(
            player,
            trigger,
            [[players[player].getForfeitTrigger("must_masturbate"), OPPONENT_LOST]]
        );
        
        players[player].preloadStageImages(players[player].stage + 2);
    }
    
    return clothing.length;
}

/************************************************************
 * Manages the dialogue triggers as player begins to strip
 ************************************************************/
function prepareToStripPlayer (player) {
    if (player == HUMAN_PLAYER) { // Never happens (currently)
        updateAllBehaviours(
            player,
            null,
            humanPlayer.gender == eGender.MALE ? MALE_HUMAN_MUST_STRIP : FEMALE_HUMAN_MUST_STRIP
        );
    } else {
        let toBeRemovedClothing = players[player].clothing.at(-1 - players[player].stage);
        if (toBeRemovedClothing.strippingItem !== undefined) {
            toBeRemovedClothing = toBeRemovedClothing.strippingItem;
        } else if (toBeRemovedClothing.strippingLayer !== undefined) {
            toBeRemovedClothing = players[player].clothing[toBeRemovedClothing.strippingLayer];
        }
        players[player].removedClothing = toBeRemovedClothing;
        const dialogueTrigger = getClothingTrigger(players[player], toBeRemovedClothing, false);
        dialogueTrigger.push(OPPONENT_STRIPPING);

        updateAllBehaviours(player, PLAYER_STRIPPING, [dialogueTrigger]);
        players[player].preloadStageImages(players[player].stage + 2);
    }
}


/**
 * @param {PlayerClothing} clothing 
 */
function StripClothingSelectionIcon (clothing) {
    this.clothing = clothing;
    this.elem = clothing.createSelectionElement();
    this.selected = false;

    $(this.elem).on("click", this.select.bind(this)).addClass("player-strip-selector").tooltip({
        title: this.clothing.tooltip(),
    });
}

StripClothingSelectionIcon.prototype.canSelect = function () {
    return !this.clothing.removed;
}

StripClothingSelectionIcon.prototype.update = function () {
    $(this.elem).removeClass("available selected");

    if (this.canSelect()) {
        $(this.elem).addClass("available");

        if (this.selected) {
            $(this.elem).addClass("selected");
        }
    }
}

StripClothingSelectionIcon.prototype.select = function () {
    if (!this.canSelect) return;
    clothingStripSelectors.forEach(function (selector) {
        selector.selected = (selector === this);
        selector.update();
    }.bind(this));

    /* enable the strip button */
    $stripButton.attr('disabled', false).attr('onclick', 'closeStrippingModal();');
}

function setupStrippingModal () {
    clothingStripSelectors = humanPlayer.clothing.map(function (clothing) {
        return new StripClothingSelectionIcon(clothing);
    });

    $stripClothing.empty().append(clothingStripSelectors.map(function (selector) {
        return selector.elem;
    }));
}

$stripModal.on('hidden.bs.modal', function () {
    if (gamePhase === eGamePhase.STRIP) {
        console.error("Possible softlock: player strip modal hidden with game phase still at STRIP");

        Sentry.captureException(new Error("Possible softlock: player strip modal hidden with phase still at STRIP"));

        allowProgression();
    }
});

/************************************************************
 * Sets up and displays the stripping modal, so that the human
 * player can select an article of clothing to remove.
 ************************************************************/
function showStrippingModal () {
    console.log("The stripping modal is being shown.");

    clothingStripSelectors.forEach(function (selector) {
        selector.selected = false;
        selector.update();
    }.bind(this));
    $stripClothing.on('show.bs.tooltip', function(ev) {
        $stripClothing.find('.player-strip-selector').not(ev.target).tooltip('hide');
    });

    /* disable the strip button */
    $stripButton.attr('disabled', true);

    /* display the stripping modal */
    $stripModal.modal({show: true, keyboard: false, backdrop: 'static'});

    $(document).keyup(clothing_keyUp);
}

/************************************************************
 * A keybound handler.
 ************************************************************/
function clothing_keyUp(e) {
    var availableSelectors = clothingStripSelectors.filter(function (selector) {
        return selector.canSelect();
    });

    if (e.key == ' ' && !$stripButton.prop('disabled')  // Space
        && !($('body').hasClass('focus-indicators-enabled') && $(document.activeElement).is('button:not(.selected)'))
        && availableSelectors.some(function (selector) { return selector.selected; })) {
        $stripButton.click();
        e.preventDefault();
    } else if (e.key >= '1' && e.key <= availableSelectors.length) { // A number key
        availableSelectors[e.key - 1].select();
    }
}

/************************************************************
 * The human player closed the stripping modal. Removes an
 * article of clothing from the human player.
 ************************************************************/
function closeStrippingModal () {
    var selector = clothingStripSelectors.find(function (selector) {
        return selector.selected;
    });

    if (!selector) {
        /* how the hell did this happen? */
        console.log("Error: there was no selected article.");
        showStrippingModal();
        return;
    }

    /* prevent double-clicking the stripping modal buttons. */
    $stripButton.attr('disabled', true).removeAttr('onclick');
    
    /* grab the removed article of clothing */
    var removedClothing = selector.clothing;

    if (!humanPlayer.getClothing().includes(removedClothing)) {
        console.log("Error: could not find clothing to remove");
        showStrippingModal();
        return;
    }

    humanPlayer.timeInStage = -1;
    humanPlayer.ticksInStage = 0;
    humanPlayer.removedClothing = removedClothing;
    humanPlayer.numStripped[removedClothing.type]++;
    if ([IMPORTANT_ARTICLE, MAJOR_ARTICLE, MINOR_ARTICLE].indexOf(removedClothing.type) >= 0) {
        humanPlayer.mostlyClothed = false;
    }

    /* determine its dialogue trigger */
    var dialogueTrigger = getClothingTrigger(humanPlayer, removedClothing, true);
    humanPlayer.removedClothing.removed = true;
    console.log(removedClothing);
    /* display the remaining clothing */
    displayHumanPlayerClothing();
    
    /* count the clothing the player has remaining */
    humanPlayer.stage++
    
    /* update label */
    if (humanPlayer.clothing.length > 0) {
        $gameClothingLabel.html("Your Remaining Clothing");
    } else {
        $gameClothingLabel.html("You're Naked");
    }
        
    /* update behaviour */
    dialogueTrigger.push(OPPONENT_STRIPPED);
    updateAllBehaviours(HUMAN_PLAYER, null, [dialogueTrigger]);

    /* allow progression */
    $stripModal.modal('hide');
    $(document).off('keyup', clothing_keyUp);
    endRound();
}

/************************************************************
 * Removes an article of clothing from an AI player. Also
 * handles all of the dialogue triggers involved in the process.
 ************************************************************/
function stripAIPlayer (player) {
    console.log("Opponent "+player+" is being stripped.");

    /* grab the removed article of clothing and determine its dialogue trigger */
    let layer = players[player].clothing.length - players[player].stage - 1;
    const removedClothing = players[player].removedClothing = players[player].clothing[layer];
    players[player].numStripped[removedClothing.type]++;
    if ([IMPORTANT_ARTICLE, MAJOR_ARTICLE, MINOR_ARTICLE].includes(removedClothing.type)) {
        players[player].mostlyClothed = false;
    }
    const dialogueTrigger = getClothingTrigger(players[player], removedClothing, true);

    players[player].removedClothing.removed = true;
    players[player].stage++;
    players[player].timeInStage = -1;
    players[player].ticksInStage = 0;
    players[player].stageChangeUpdate();

    /* update behaviour */
    dialogueTrigger.push(OPPONENT_STRIPPED);
    updateAllBehaviours(player, PLAYER_STRIPPED, [dialogueTrigger]);

    layer--;
    if (layer >= 0 && players[player].clothing[layer].type == "skip") {
        while (layer >= 0 && players[player].clothing[layer--].type == "skip") {
            players[player].stage++;
            players[player].stageChangeUpdate();
        }
        updateGameVisual(player);
    }

    for (const c of players[player].clothing) {
        if (c.fromStage == players[player].stage) {
            c.fromStage = undefined;
        }
    }
}

/************************************************************
 * Determines whether or not the provided player is winning
 * or losing at the end and returns the appropriate dialogue trigger.
 ************************************************************/
function determineForfeitSituation (player) {
    /* check to see how many players are out */
    for (var i = 0; i < players.length; i++) {
        if (players[i] && players[i].out) {
            if (players[i].out) {
                return PLAYER_MUST_MASTURBATE;
            }
        }
    }
    return PLAYER_MUST_MASTURBATE_FIRST;
}

/************************************************************
 * Removes an article of clothing from the selected player.
 * Also handles all of the dialogue triggers involved in the
 * process.
 ************************************************************/
function stripPlayer (player) {
    if (player == HUMAN_PLAYER) {
        showStrippingModal();
    } else {
        stripAIPlayer(player);
        /* allow progression */
        endRound();
    }
}

/************************************************************
 * Counts the number of players in a certain stage
 ************************************************************/
function getNumPlayersInStage(stage) {
    return players.countTrue(function(player) {
        return player.checkStatus(stage)
    });
}

/************************************************************
 * Updates .biggestLead of the leader
 ************************************************************/
function updateBiggestLead() {
    var sortedPlayers = players.slice().sort(function(a, b) {
        return b.countLayers() - a.countLayers();
    });
    if (sortedPlayers[0].countLayers() - sortedPlayers[1].countLayers() > sortedPlayers[0].biggestLead) {
        sortedPlayers[0].biggestLead = sortedPlayers[0].countLayers() - sortedPlayers[1].countLayers();
    }
}

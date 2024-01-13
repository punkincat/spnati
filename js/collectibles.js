/********************************************************************************
 This file contains the Collectible class and collectible-related functions not
 directly related to the gallery screen.
 ********************************************************************************/

function Collectible(xmlElem, player) {
    this.id = xmlElem.attr('id');
    this.image = xmlElem.attr('img');
    this.thumbnail = xmlElem.attr('thumbnail') || this.image;
    this.status = xmlElem.attr('status');
    this.title = unescapeHTML(xmlElem.children('title').text());
    this.subtitle = unescapeHTML(xmlElem.children('subtitle').text());
    this.unlock_hint = unescapeHTML(xmlElem.children('unlock').text());
    this.text = unescapeHTML(xmlElem.children('text').html());
    this.detailsHidden = xmlElem.children('hide-details').text() === 'true';
    this.hidden = xmlElem.children('hidden').text() === 'true';
    this.counter = parseInt(xmlElem.children('counter').text(), 10) || undefined;

    if (this.counter <= 0) this.counter = undefined;

    if (player) {
        this.source = player.metaLabel;
        this.player = player;
    } else {
        this.source = 'The Inventory';
        this.player = undefined;
    }

    this.clothing = null;

    var clothingElems = xmlElem.children("clothing").map(function () { return $(this); }).get();
    if (clothingElems.length > 0) {
        var $elem = clothingElems[0];
        var generic = $elem.attr('generic');
        var name = $elem.attr('name');
        var type = $elem.attr('type');
        var position = $elem.attr('position');
        var plural = $elem.attr('plural');
        plural = (plural == 'null' ? null : plural == 'true');

        var genders = $elem.attr('gender') || "all";
        var image = $elem.attr('img') || this.image;

        var newClothing = new PlayerClothing(name, generic, type, position, image, plural, this.id, genders, this);
        this.clothing = newClothing;

        if (!this.status || includedOpponentStatuses[this.status]) {
            PLAYER_CLOTHING_OPTIONS[newClothing.id] = newClothing;
        }
    }
}

Collectible.prototype.isUnlocked = function (ignoreUnlockAllOption) {
    if (COLLECTIBLES_UNLOCKED && !ignoreUnlockAllOption) return true;

    var curCounter = save.getCollectibleCounter(this);
    if (this.counter) {
        return curCounter >= this.counter;
    } else {
        return curCounter > 0;
    }
}

Collectible.prototype.getCounter = function () {
    var ctr = save.getCollectibleCounter(this);
    return (this.counter && ctr > this.counter) ? this.counter : ctr;
}

Collectible.prototype.unlock = function () {
    save.setCollectibleCounter(this, this.counter || 1);
}

Collectible.prototype.incrementCounter = function (inc) {
    var newCounter = save.getCollectibleCounter(this) + inc;

    if (this.counter && newCounter > this.counter)
        newCounter = this.counter;

    save.setCollectibleCounter(this, newCounter);
}

Collectible.prototype.setCounter = function (val) {
    if (this.counter && val > this.counter)
        val = this.counter;

    save.setCollectibleCounter(this, val);
}

Collectible.prototype.display = function () {
    var offlineIndicator = "";
    if (this.status && this.status != "online") {
        offlineIndicator = "[Offline] ";
    }

    if ((!this.detailsHidden && !this.hidden) || this.isUnlocked()) {
        $collectibleTitle.html(offlineIndicator + this.title);
        $collectibleSubtitle.html(this.subtitle).show();
    } else {
        $collectibleTitle.html(offlineIndicator + "[Locked]");
        $collectibleSubtitle.html("").hide();
    }

    $collectibleCharacter.text(this.source);
    $collectibleUnlock.html(this.unlock_hint);

    if (this.counter) {
        var curCounter = this.getCounter();
        var pct = Math.round((curCounter / this.counter) * 100);

        $collectibleProgressBar
            .attr('aria-valuenow', pct)
            .css('width', pct+'%');
        $collectibleProgressContainer.show();

        $collectibleProgressText.text('('+curCounter+' / '+this.counter+')').show();
    } else {
        $collectibleProgressContainer.hide();
        $collectibleProgressText.hide();
    }

    $collectibleTextPane.show();

    if (this.isUnlocked()) {
        $collectibleText.html(this.text);
        $collectibleTextContainer.show();

        if (this.image) {
            $collectibleImage.attr('src', this.image);
            $collectibleImagePane.show();
        } else {
            $collectibleImagePane.hide();
        }
    } else {
        $collectibleTextContainer.hide();
        $collectibleImagePane.hide();
    }
};

Collectible.prototype.listElement = function () {
    if (this.status && !includedOpponentStatuses[this.status]) {
        return null;
    }

    if (this.hidden && !this.isUnlocked()) {
        return null;
    }

    var baseElem = $('<div class="gallery-pane-list-item bordered"></div>');
    var imgElem = $('<img class="gallery-pane-item-icon">');
    var titleElem = $('<div class="gallery-pane-item-title"></div>');
    var subtitleElem = $('<div class="gallery-pane-item-subtitle"></div>');

    var offlineIndicator = "";
    if (this.status && this.status != "online") {
        offlineIndicator = "[Offline] ";
    }

    if (!this.detailsHidden || this.isUnlocked()) {
        titleElem.html(offlineIndicator + this.title);
        subtitleElem.html(this.subtitle);
    } else {
        titleElem.html(offlineIndicator + "[Locked]");
        subtitleElem.html(this.unlock_hint);
    }

    if (this.counter) {
        var curCounter = this.getCounter();
        var curSubtitle = subtitleElem.html();
        subtitleElem.html(curSubtitle + ' ('+curCounter+' / '+this.counter+')');
    }

    if (this.isUnlocked()) {
        imgElem.attr('src', this.thumbnail);
    } else {
        imgElem.attr('src', "img/unknown.svg");
    }

    baseElem.append(imgElem, titleElem, subtitleElem).click(this.display.bind(this));
    return baseElem;
};

Collectible.prototype.displayInfoModal = function () {
    $('#collectible-info-thumbnail').attr('src', this.thumbnail);
    $('#collectible-info-title').html(this.title);
    $('#collectible-info-subtitle').html(this.subtitle);

    $collectibleInfoModal.modal('show');

    /* Hide the modal if the user clicks anywhere outside of it. */
    $('.modal-backdrop').one('click', function () {
        $collectibleInfoModal.modal('hide');
    })
}

function loadAllCollectibles() {
    console.log("Loading all collectibles");
    return loadGeneralCollectibles().then(function () {
        beginStartupStage("Collectibles");

        var nLoaded = 0;
        return Promise.all(loadedOpponents.map(function (opp) {
            var ret = opp ? opp.fetchCollectibles() : immediatePromise();
            return ret.then(function () {
                updateStartupStageProgress(++nLoaded, loadedOpponents.length);
            });
        }));
    });
}

function loadGeneralCollectibles () {
    return metadataIndex.getFile('opponents/general_collectibles.xml').then(function($xml) {
        humanPlayer.collectibles = generalCollectibles;
        $xml.children('collectible').each(function (idx, elem) {
            generalCollectibles.push(new Collectible($(elem), undefined));
        });
        $xml.children('case:not([disabled="1"])').each(function (idx, elem) {
            const trigger = $(elem).attr('trigger');
            $(elem).attr('hidden', 'true');
            const c = new Case($(elem), trigger);
            if (!generalCollectibleCases.has(trigger)) {
                generalCollectibleCases.set(trigger, []);
            }
            generalCollectibleCases.get(trigger).push(c);
        });
    }).catch(function (err) {
        console.error("Failed to load general collectibles");
        captureError(err);
    });
}

function evaluateGeneralCollectibleCases (triggers, target) {
    const cases = [];
    if (!Array.isArray(triggers)) {
        triggers = [triggers];
    } else {
        triggers = triggers.flat();
    }
    if (triggers[0] !== DEALING_CARDS) {
        triggers.push(GLOBAL_CASE);
    }
    triggers.forEach(function (trigger) {
        cases.push(...(generalCollectibleCases.get(trigger) || []));
    });

    if (cases.length <= 0) return;

    /* Sort hidden cases in order of descending custom priority. */
    cases.sort(function (a, b) {
        return b.priority - a.priority;
    });

    cases.forEach(function (c) {
        if (c.checkConditions(humanPlayer, target)) {
            humanPlayer.applyHiddenStates(c, target);
        }
    });

    if (humanPlayer.pendingCollectiblePopups.length) {
        $('#collectible-button-0').show();
    }
}

function onGeneralCollectibleIndicatorClick () {
    if (humanPlayer.pendingCollectiblePopups.length == 0) return;
    humanPlayer.pendingCollectiblePopups.shift().displayInfoModal();

    if (humanPlayer.pendingCollectiblePopups.length == 0) {
        $('#collectible-button-0').hide();
    }
}
$('#collectible-button-0').click(onGeneralCollectibleIndicatorClick);

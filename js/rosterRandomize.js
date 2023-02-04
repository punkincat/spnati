/* Irwin-Hall distribution */
function randomNormal(mean, std) {
    var ret = 0;
    for (let i = 0; i < 12; i++) {
        ret += Math.random();
    }
    ret -= 6;

    return (ret * std) + mean;
}

/**
 * Partition characters into groups based on franchise magnetism rules.
 * nearby characters from the same franchise will stick together even after
 * the group is shuffled as a whole.
 * 
 * @param {Opponent[]} opponents 
- * @returns {Opponent[][]}
 */
function computeMagnetismGroups(opponents) {
    var groups = [];

    while (opponents.length > 0) {
        let curOpp = opponents.shift();
        let group = [curOpp];
        if (curOpp.effectiveScore > 0 && curOpp.magnetismTag) {
            let j = 0;
            for (let i = 0; i < 10; i++) {
                if (j >= opponents.length) break;
                if (opponents[j].effectiveScore < 0) break;
                if (opponents[j].magnetismTag == curOpp.magnetismTag) {
                    group.push(opponents.splice(j, 1)[0]);
                } else {
                    j++;
                }
            }
        }

        groups.push(group);
    }

    return groups;
}

/**
 * 
 * @param {Opponent[]} opponents 
- * @returns {Opponent[]}
 */
function applyFeaturedSortRules(opponents) {
    var tmp = [];
    var i = 0;
    var foundMale = false;
    opponents = opponents.slice();

    while (tmp.length < 10 && i < opponents.length) {
        let curOpp = opponents[i];
        if (curOpp.magnetismTag && tmp.some((opp) => opp.magnetismTag == curOpp.magnetismTag)) {
            i++;
            continue;
        }

        foundMale = foundMale || (curOpp.metaGender === "male");
        tmp.push(opponents.splice(i, 1)[0]);
    }

    /* Apply magnetism to remaining opponents and recombine again. */
    var ret = Array.prototype.concat.apply(tmp, computeMagnetismGroups(opponents));

    /* If there isn't already a male in the top 10, pull one up. */
    if (!foundMale) {
        for (let i = 10; i < ret.length; i++) {
            if (ret[i].metaGender === "male") {
                /* Move opponents[i] to opponents[9]. */
                let opp = ret.splice(i, 1)[0];
                ret.splice(9, 0, opp);
                break;
            }
        }
    }

    return ret;
}

function randomizeRosterOrder(startStd, endStd) {
    startStd = (startStd !== undefined) ? startStd : 0.3;
    endStd = (endStd !== undefined) ? endStd : 0.15;

    var roster = loadedOpponents.slice().filter(function (opp) {
        return opp && !!opp.rosterScore;
    }).sort(function (a, b) {
        return b.rosterScore - a.rosterScore;
    });

    var rosterLength = roster.slice().filter(function (opp) {
        return opp && opp.rosterScore > 0;
    }).length;

    roster.forEach(function (opp, idx) {
        if (opp.rosterScore > 0) {
            let curStd = ((idx / rosterLength) * (endStd - startStd)) + startStd;
            let multiplier = Math.exp(randomNormal(0, curStd));
            opp.effectiveScore = opp.rosterScore * multiplier;
        } else {
            opp.effectiveScore = opp.rosterScore;
        }
    });
}

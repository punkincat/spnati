#!/bin/bash -e

mkdir -p .public/opponents .public/img .public/js
cp -r css fonts player index.html version-info.xml backgrounds.xml events.xml cards.xml .public

npm ci
./node_modules/.bin/babel js --out-dir .public/js --copy-files --source-maps inline --ignore "js/fileIndex.js","js/core-js.3.35.0.js","js/core-js.3.35.0.min.js","js/core-js.3.35.0.min.js.map","js/fetch.js","js/unorm.js","js/bootstrap*","js/jquery*","js/pako*.js","js/js.cookie.js","js/ResizeObserver.js","js/sentry*.js"

# Copy non-recursively to exclude the backgrounds folder.
cp img/* .public/img

# Copy card images recursively.
cp -r img/cards .public/img

# Set build timestamp in version-info.xml.
# `date +%s` returns timestamp in seconds; add %N and cut to include milliseconds.
sed "s/__BUILD_TIMESTAMP/$(date +%s%N | cut -b1-13)/g" version-info.xml > .public/version-info.xml

sed "s/__CI_COMMIT_SHA/${CI_COMMIT_SHA}/g; s/__VERSION/${VERSION}/g" prod-config.xml > .public/config.xml
cp opponents/listing.xml .public/opponents
cp opponents/general_collectibles.xml .public/opponents

# Copy online background images and set initial background to display during
# loading.
python3 deploy-scripts/copy_backgrounds.py .public/

# tar may be the easiest way to copy an arbitrary
# list of files, keeping the directory structure.
# Include *.js and *.css to accommodate Monika.
find `python3 opponents/list_opponents.py` -regextype egrep -iregex '.*\.(png|gif|jpe?g|xml|js|css|[ot]tf|woff2?)' | tar -cT - | tar -C .public -x

# Copy alternate costume files for deployment.
python3 deploy-scripts/copy_alternate_costumes.py .public/ ./ all

# Combine roster metadata, tags lists, collectible definitions, and costume
# definitions for all deployed opponents to speed up loading for the online version.
METADATA_INDEX_PATH=$(python3 deploy-scripts/compile_xml_index.py --production .public opponents/metadata.index 'opponents/general_collectibles.xml' 'opponents/*/collectibles.xml' 'opponents/*/meta.xml' 'opponents/*/tags.xml' 'opponents/reskins/*/costume.xml')
sed "s/__METADATA_XML_INDEX/${METADATA_INDEX_PATH}/g" js/fileIndex.js | ./node_modules/.bin/babel -f "js/fileIndex.js" --source-maps inline --out-file .public/js/fileIndex.js

# Rename JS and core game CSS for cache-busting purposes.
python3 deploy-scripts/cache_bust.py .public/

python3 opponents/fill_linecount_metadata.py .public/opponents
python3 deploy-scripts/fill_update_timestamps.py .public/opponents opponents
python3 opponents/gzip_dialogue.py .public/opponents/*/behaviour.xml
python3 opponents/analyze_image_space.py .public/opponents

#!/bin/bash

gh release create "$tag" \
    --repo="$GITHUB_REPOSITORY" \
    --title="${GITHUB_REPOSITORY#*/} ${tag#v}" \
    --generate-notes

gh release upload "$tag" fsglue.zip
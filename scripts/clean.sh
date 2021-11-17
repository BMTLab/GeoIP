#!/bin/bash

echo "Cleaning projects"
find .. -name "build" -print0 | xargs -r0 -- rm -r
find .. -name "bin" -print0 | xargs -r0 -- rm -r
find .. -name "obj" -print0 | xargs -r0 -- rm -r

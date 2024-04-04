#!/bin/zsh

cd datadons-app

echo "Cleaning app..."
rm -r node_modules
npm install --legacy-peer-deps
npm start
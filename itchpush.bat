set /p Version=<Builds/version.txt

butler push Builds/Windows/%Version%/ rhizomepublishing/antelopeup:windows --userversion-file Builds/version.txt
butler push Builds/Mac/%Version%/ rhizomepublishing/antelopeup:osx --userversion-file Builds/version.txt
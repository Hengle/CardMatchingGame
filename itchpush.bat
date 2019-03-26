set /p Version=<Builds/version.txt

butler -i %UserProfile%/.config/itch/rhizome push Builds/Windows/%Version%/ rhizomepublishing/antelopeup:windows --userversion-file Builds/version.txt
butler -i %UserProfile%/.config/itch/rhizome push Builds/Mac/%Version%/ rhizomepublishing/antelopeup:osx --userversion-file Builds/version.txt

# CatMash API

This is only the API repo of my CatMash.

The VueJS App is hosted on this repo :
https://github.com/TheOnlyMrFlow/CatMashApp

## The routes

### /cats (get)

=> Display all the cats

Use 

/cats?sortBy=elo

to have them in the ranking order

### /cats/pick (get)

=> Select matchup of 2 cats

### /cats/{idWinner}/mashes/{idLoser} (patch)

=> To tell the winner of a match



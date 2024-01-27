# Tam-VR
<b>TamVR</b> is a Unity VR application for exploring genealogy or a pedigree graph in virtual reality. The family tree graph is situated on a landscape, where the height of the terrain corresponds to the birth year of each individual. Connections between families and individuals are represented by roads on the terrain, families by mills, and individual persons by houses. Each element is accompanied with a tooltip showing important information about a person such as name, family name, birth year, death year, etc. The player can navigate the terrain, track the year (height) of where the player is currently, use the PathFinder menu to find connections between individuals, set a visible year (height) contour, use a telescope, and adjust the size of the terrain and UI panels.

TamVR is a VR adaptation of a web application <b><a href="https://github.com/rpreiner/tam">Topographic Attribute Maps</a></b>. The application uses <b><a href="https://de.wikipedia.org/wiki/GEDCOM">GEDCOM</a></b> (FamilySearch GEDCOM), an open file format for genealogical data. GEDCOM allows to exchange genealogical data between different systems.

NOTE: The code will be released upon the publication of the associated paper.

<p align="center">
<img src="https://github.com/dedovskaya/Tam-VR/assets/71874540/c1abb131-7c59-4fcb-bf6c-af42614f4fc2" width="700">
</p>
<p align="center">
<img src="https://github.com/dedovskaya/Tam-VR/assets/71874540/9dc82d7b-2955-4714-8c89-170e110dcc5d" width="700">
</p>


## How it works
The TamVR is a project aimed at bringing to life the intricate connections between individuals and their families. Through the usage of GEDCOM or JSON data, the application transforms genealogical information into a network consisting of individuals, families, and connections between them. The network then is transformed into a force graph minimizing the overlaps of the edges and frozen to store the positions of nodes. This frozen graph is then transformed into a heightmap using a birth year as a height for each node. Depending on the birth year of each individual a terrain is generated (the later a person is born, the higher the point in the terrain). Terrain and the network are then rendered within a virtual reality environment, creating an immersive and interactive experience that allows users to explore and understand family relationships in a whole new way.

<p align="center">
<img src="https://github.com/dedovskaya/Tam-VR/assets/71874540/931e3116-eeea-4acc-94fc-192ae7147088" width="700"></p>

<p align="center">
<img src="https://github.com/dedovskaya/Tam-VR/assets/71874540/6e834467-1cb3-492a-9a46-4aa80cd2cf2a" width="700"></p>

### Terrain Generation


Complete procedural terrain includes automatic terrain decoration, with texturing determined by the height and slope of the mesh with the additional splatmaps for road texture. Subsequent layers are applied to the terrain based on the splatmaps, facilitating the placement of decorative objects in accordance with the generated map.

<p align="center">
<img src="https://github.com/dedovskaya/Tam-VR/assets/71874540/7a110c9d-0dd6-4fdc-a5ea-c0292f2025b5" width="500"></p>


## Hand Menu
Hand menu allows you to control navigation options, interaction tools, and customization features.


### Minimap
Minimap allows the players to see a 2D map with the network and player position marker which updates when the player is moving.

  <p align="center">
  <img src="https://github.com/dedovskaya/Tam-VR/assets/71874540/834da6cd-6f3a-4af7-90a5-44aaf34382e5" width="700"></p>

  
### Path Finder 
A menu where the player can select two or more individuals and calculate the shortest path based on the graph connectivity (BFS) or based on Euclidean distance through the terrain (Dijkstra algorithm).
  <p align="center">
  <img src="https://github.com/dedovskaya/Tam-VR/assets/71874540/3a8d44a9-b8ba-4aa8-9c37-e63b33862195" width="700"></p>

  
### Height Contour
The user can spawn the contour that shows the height in the terrain, make the contour follow the user, or adjust it manually.
  <p align="center">
  <img src="https://github.com/dedovskaya/Tam-VR/assets/71874540/ef4f22c6-57f4-45a2-97d9-4677e09121f9" width="700"></p>


### Telescope 
The user can spawn a telescope and grab it to see the distant scene elements.
  <p align="center">
  <img src="https://github.com/dedovskaya/Tam-VR/assets/71874540/80e29295-2863-49ae-9791-baa6067cc2f5" width="700"></p>


### Terrain and UI Size
The user can adjust the terrain size and the size of the UI panels.

## Unity Version
Unity 2021.3.20f or later

## Author
Ekaterina Baikova, 2023

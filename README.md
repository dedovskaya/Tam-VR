# Tam-VR
VR-based Exploration of Topographical Attribute Maps
TamVR, a VR adaptation of the web application featuring topographic attribute maps. 
![image1](https://github.com/dedovskaya/Tam-VR/assets/71874540/c1abb131-7c59-4fcb-bf6c-af42614f4fc2)
![tam2](https://github.com/dedovskaya/Tam-VR/assets/71874540/9dc82d7b-2955-4714-8c89-170e110dcc5d)


## How it works
The Gedcom file is transformed into a dynamic force graph, visually representing intricate family connections. In this immersive experience, each node corresponds to an individual, while each circle encapsulates a family unit. The vertical dimension is tied to birth year, creating a distinctive height profile. Employing interpolation techniques, data points are seamlessly connected to form a detailed heightfield, enhancing the immersive visualization of familial relationships.
The TamVR is a project aimed at bringing to life the intricate connections between individuals and their families. Through the utilization of GEDCOM or JSON data, the application transforms genealogical information into a network consisting of individuals, families, and connections between them. Depending on the birth year of each individual a terrain is generated (the later a person is born, the higher the point in the terrain). Terrain and the network are then rendered within a virtual reality environment, creating an immersive and interactive experience that allows users to explore and understand family relationships in a whole new way.
![image7](https://github.com/dedovskaya/Tam-VR/assets/71874540/931e3116-eeea-4acc-94fc-192ae7147088)
![image8](https://github.com/dedovskaya/Tam-VR/assets/71874540/6e834467-1cb3-492a-9a46-4aa80cd2cf2a)
![level](https://github.com/dedovskaya/Tam-VR/assets/71874540/f367ae80-b3d5-4107-be5b-2b1670bfbb11)


## Terrain Generation
![tam1](https://github.com/dedovskaya/Tam-VR/assets/71874540/7a110c9d-0dd6-4fdc-a5ea-c0292f2025b5)

Complete procedural terrain generation powered by tam-web, allowing users to select any available file for output. The process includes automatic terrain decoration, with splatmap generation determined by the height and slope of the mesh. Subsequent layers are applied to the terrain based on the splatmap, facilitating the placement of decorative objects in accordance with the generated map.

## Hand Menu
Hand menu allows you to control navigation options, interaction tools, and customization features.
![image10](https://github.com/dedovskaya/Tam-VR/assets/71874540/68241976-7b51-4363-8905-b554ba263b5a)

## Finding the Path
![pathfinder](https://github.com/dedovskaya/Tam-VR/assets/71874540/0d84ca86-7836-4f68-bf4e-df53dc432933)

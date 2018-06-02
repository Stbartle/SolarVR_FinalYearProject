# SolarVR

This repository represents the files used for the development of the software tool SolarVR.
There are three folders of interest related to this project. 

1: SolarVR-Unity, contains the Unity project files for the main application, for this project to operate currently, this folder must be located on the users desktop.

2: Project, meant to be located in side of the computers documents folder, this enables the operation of the communications method, and in turn the Matlab power calculation operations.

3: Video, this folder simply contains a single video, this video shows some limited capabilities of the project.

For this project to function, the a line of code must be changed in the NamedPipesServerStream.cs file, this line controls the launching of the communications channel, linking to a shortcut to the communications application located in the project folder.


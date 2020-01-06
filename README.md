# Fin Pen Toronto

Presented at the [2018 FinPen: NEXT - Year End Show](http://yearendshow.schoolofdesign.ca/finpen/) 

The [NEXT - Year End Show](http://yearendshow.schoolofdesign.ca/) is held annually at the [School of Design, George Brown College (GBC)](https://www.georgebrown.ca/design/), located Downtown Toronto at the Waterfront Campus, where senior year design students showcase their final year projects.

# Team

Lead Developer (Volunteer) -- [Christopher Cabral](https://github.com/Cabralcm)

Lead Interaction Designer (GBC) -- [Soonmin Jung](https://www.soonminjung.com/)

Interaction Designer (GBC) -- [Hyeji An](https://www.behance.net/HYEJIAN?tracking_source=search%7Chyeji%20an)

Interaction Designer (GBC) -- [Daeyoung Kim](https://www.behance.net/44mg_dayoung/projects)

# Installation

Installation steps for Steps (1) to (5) can be found on: [Microsoft Hololens Website](https://docs.microsoft.com/en-us/windows/mixed-reality/install-the-tools)

Step (5), Unity Game Engine, is required for extracting the compressed ```.unity``` file, and compiling the project. Please select the recommended version of Unity Game Engine found at the bottom of the website: [Microsoft Hololens Website](https://docs.microsoft.com/en-us/windows/mixed-reality/install-the-tools). **Strongly recommended to install the Long Term Support (LTS) version of Unity**.

>Used the 2017 LTS version of Unity for this project.

## Basic requirements:
1) [Windows 10](https://www.microsoft.com/en-ca/software-download/windows10)
2) [Visual Studio](https://visualstudio.microsoft.com/downloads/)
   - Community version is FREE
   - Used 2017 version for this project, but most recent version should be fine
3) [Windows 10 SDK](https://developer.microsoft.com/en-us/windows/downloads/windows-10-sdk)
4) **Hardware** [Microsoft Hololens (Gen 1)](https://docs.microsoft.com/en-us/hololens/hololens1-hardware), or **Software** [Hololens Emulator](https://go.microsoft.com/fwlink/?linkid=2065980)
    - **Strongly Recommend** using the physical Microsoft Hololens (Gen 1) for best user experience, and for all features to work!

If any broken individual links, try: [Microsoft Hololens Website](https://docs.microsoft.com/en-us/windows/mixed-reality/install-the-tools)
    
## Game Engine requirements:
5) [Unity Game Engine](https://docs.microsoft.com/en-us/windows/mixed-reality/install-the-tools#choose-your-engine)
6) [Mixed Reality Toolkit (MMTK)](https://github.com/Microsoft/MixedRealityToolkit-Unity/releases)

# Running Project on Hololens

**Strongly recommend** having an understanding of how to build Hololens projects in Unity, and how to compile and upload to Hololens device from Visual Studio. Instead of writing an inferior guide here, please follow the first few [Microsoft Hololens - Mixed Reality Tutorials](https://docs.microsoft.com/en-us/windows/mixed-reality/holograms-100) for a professional guide.

## Basic Steps to Build, Compile and Upload Project to Hololens

1) Open Unity Game Engine
2) Download ```.unity``` package file from this Repository
3) Import ```.unity``` package into Unity. ([How to Import Unity Assets File](https://docs.unity3d.com/560/Documentation/Manual/AssetPackages.html))
4) Open ```Main``` Scene from Assets Package
5) Compile and build the scene in Unity, which will correspondingly open up Visual Studio.
6) Go to Visual Studio (should already be open)
7) Physically connect Hololens to computer (can also upload via Wifi, but this is much slower), make sure the Hololens appears as the device you wish to upload the code to.
8) Build/Upload project to device (pressing the Run button). Make sure the settings are correct. Please consult Microsoft tutorials above for help.
9) After uploading, turn on hololens, locate app, run and enjoy!

# Interaction Gestures

Draw with your finger while surrounded by the Toronto city-scape! 

**Strongly Recommended** - Follow Hololens Gestures tutorial that can be run within the Microsoft Hololens to learn the different **default** Hololens gestures.

**Gestures**
1) ```Tap/Click Gesture``` - Start/Stop drawing with your finger (trail that follows your finger)
2) ```Index Finger``` with arm extended out in front of you (your default pointing gesture). This is the gesture you draw with!


**Voice Commands**
2) **Speak:** ```Color <COLOR NAME>``` - Change current drawing colour. Color option commands include:
   - ```Color Red```
   - ```Color Blue```
   - ```Color Green```
   - ```Color White```
   - ```Color Yellow```
   - ```Color Rainbow``` --> constantly iterates through the colors above to generate a "Rainbow" trail
3) **Speak:** ```Start Drawing``` - Start drawing with your finger.
4) **Speak:** ```Stop Drawing``` - Stop drawing with your finger.
5) **Speak:** ```Snow``` - Starts/Stops "Falling Snow" effect.
6) **Speak:** ```Clear All``` - Removes/Deletes all drawings/points.



# Additional Hololens Resources:
1) [Microsoft Hololens - Mixed Reality Tutorials](https://docs.microsoft.com/en-us/windows/mixed-reality/holograms-100)
2) [Lynda Course - Hololens (Gen 1)](https://www.lynda.com/Windows-tutorials/App-Development-Microsoft-HoloLens/587658-2.html)




   

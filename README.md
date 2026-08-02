# Verdantia 🌿

An immersive Augmented Reality (AR) application built using the Unity Engine that brings a virtual garden into your physical space. Experience nature right in your room with real-time environment interaction.

---

## Features

* **Plane Detection:** Automatically scans and detects horizontal (and vertical) surfaces in the real world to establish a ground plane for your garden.
* **Object Instantiation:** Seamlessly spawn and place interactive garden elements, plants, and virtual assets onto detected planes with precise tap controls.
* **Firebase Authentication:** Secure user management system featuring user sign-up, login, and session handling to keep your custom garden progress saved.
* **Dynamic Main Menu:** A polished user interface and menu flow built for seamless transition between authentication and the AR experience.

---

## Built With

* [Unity Engine](https://unity.com/) - Core development platform.
* **AR Foundation / ARCore / ARKit** - Cross-platform AR framework for plane tracking and raycasting.
* **Firebase SDK** - Backend infrastructure for user authentication.
* **C#** - Core programming language.

---

## Prerequisites

Before you begin, ensure you have the following installed and configured:

* **Unity Editor:** Version compatible with the current project settings (check `ProjectSettings/ProjectVersion.txt`).
* **AR-Compatible Device:** * Android device with ARCore support.
    * iOS device with ARKit support.
* **Firebase Project:** A Firebase project set up via the Firebase Console to download your configuration files (`google-services.json` for Android / `GoogleService-Info.plist` for iOS).

---

## Getting Started

1.  **Clone the Repository:**
    ```bash
    git clone https://github.com/Bharath-serman/Verdantia.git
    ```

2.  **Open in Unity:**
    * Open the Unity Hub.
    * Click **Add** and select the cloned `Verdantia` folder.
    * Launch the project (allow Unity to resolve package dependencies).

3.  **Setup Firebase:**
    * Place your downloaded `google-services.json` or `GoogleService-Info.plist` into the `Assets/` directory.

4.  **Build and Run:**
    * Go to `File > Build Settings`.
    * Switch to your target platform (Android/iOS).
    * Ensure the `MainMenu` and AR scenes are added to the **Scenes In Build** list.
    * Click **Build and Run** with your device connected via USB.

---

## Screenshots

<img width="537" height="945" alt="image" src="https://github.com/user-attachments/assets/f59031b6-58cd-4da7-8b07-4087e787a7bb" />
<img width="540" height="952" alt="image" src="https://github.com/user-attachments/assets/d8e09bf6-4da6-4e19-ba64-85ea31b097bc" />
<img width="537" height="953" alt="image" src="https://github.com/user-attachments/assets/89c0beae-f6af-418a-89b8-d055b3b05076" />
<img width="540" height="957" alt="image" src="https://github.com/user-attachments/assets/e3cf426e-1bc6-46a6-9027-87a2b7aa836c" />

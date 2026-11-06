# MOBA 1v1 Offline Map Viewer (Unity 2020.1.14f1)
Extracted from user-supplied UnityMobaDemo source. Contains map_101 and map_102, their transitive non-script assets, mobile orbit viewer, and GitHub Actions build workflow.

## Android build
1. Push all files including `.github` to a new GitHub repository (repository root = this folder).
2. Configure Unity licensing for GameCI. `UNITY_LICENSE`, `UNITY_EMAIL`, and `UNITY_PASSWORD` secrets are referenced; follow https://game.ci/docs/github/activation for current supported activation procedure. Unity Personal licensing can require additional steps; the old Unity 2020 editor may not activate on current CI.
3. Actions > Build MOBA Map Viewer APK > Run workflow.
4. On success download the artifact `MOBA-Map-Viewer-APK` and install APK on Android.

This is an untested build candidate, not a guaranteed APK. Scenes may contain missing script references (removed at build), unsupported shaders or older Unity assets. Inspect build logs. No server or combat gameplay included. Camera: drag to orbit, pinch to zoom; top buttons switch scenes.

The source archive contains third-party assets. Confirm asset licenses before redistribution or publishing.

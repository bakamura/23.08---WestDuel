#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GeneralSoundTrigger))]
[CanEditMultipleObjects]
public class GeneralSoundTriggerInspector : Editor
{
    //general variables
    SerializedProperty soundType, sfxSoundInterval, soundBasedOnVelocity/*, baseAudioSourceRangeMultiplyer*/;
    //trigger enter variables
    SerializedProperty playSoundOnTriggerEnter, musicConfigOnTriggerEnter, sfxConfigOnTriggerEnter;
    //trigger exit variables
    SerializedProperty playSoundOnTriggerExit, musicConfigOnTriggerExit, sfxConfigOnTriggerExit;
    //collision enter variables
    SerializedProperty playSoundOnCollisionEnter, musicConfigOnCollisionEnter, sfxConfigOnCollisionEnter;

    SerializedProperty playSoundOnStart, musicConfigOnStart, sfxConfigOnStart;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(soundType, new GUIContent("Sound Type"));

        EditorGUILayout.PropertyField(playSoundOnTriggerEnter, new GUIContent("Play On Trigger Enter"));
        EditorGUILayout.PropertyField(playSoundOnTriggerExit, new GUIContent("Play On Trigger Exit"));
        EditorGUILayout.PropertyField(playSoundOnStart, new GUIContent("Play On Start"));

        //GeneralSoundTrigger temp = (GeneralSoundTrigger)serializedObject.targetObject;

        if (soundType.enumValueIndex == (int)SoundManager.SoundTypes.SFX)
        {
            //if (temp.gameObject.GetComponent<AudioSource>()) DestroyImmediate(temp.gameObject.GetComponent<AudioSource>());

            //EditorGUILayout.PropertyField(baseAudioSourceRangeMultiplyer, new GUIContent("Audio range Multiplyer"));
            EditorGUILayout.PropertyField(playSoundOnCollisionEnter, new GUIContent("Play On Collision Enter"));
            EditorGUILayout.PropertyField(soundBasedOnVelocity, new GUIContent("Sound Based On Velocity", "if true the volume will be based on the velocity of the object that will colide with this object. The Volume Values doesn't affect anything if true"));
            EditorGUILayout.PropertyField(sfxSoundInterval, new GUIContent("Sfx Sound Interval", "the interval between the sounds"));

            if (playSoundOnTriggerEnter.boolValue)
            {
                GUILayout.Label("Sfx Trigger Enter Configurations", EditorStyles.largeLabel);
                EditorGUILayout.PropertyField(sfxConfigOnTriggerEnter, new GUIContent("Sfx Sounds Cofigurations"));
            }

            if (playSoundOnTriggerExit.boolValue)
            {
                GUILayout.Label("Sfx Trigger Exit Configurations", EditorStyles.largeLabel);
                EditorGUILayout.PropertyField(sfxConfigOnTriggerExit, new GUIContent("Sfx Sounds Cofigurations"));
            }

            if (playSoundOnCollisionEnter.boolValue)
            {
                GUILayout.Label("Sfx Collision Enter Configurations", EditorStyles.largeLabel);
                EditorGUILayout.PropertyField(sfxConfigOnCollisionEnter, new GUIContent("Sfx Sounds Cofigurations"));
            }

            if (playSoundOnStart.boolValue)
            {
                GUILayout.Label("Sfx On Start Configurations", EditorStyles.largeLabel);
                EditorGUILayout.PropertyField(sfxConfigOnStart, new GUIContent("Sfx Sounds Cofigurations"));
            }
        }
        //the trigger activates music
        else
        {
            //if (!temp.gameObject.GetComponent<AudioSource>()) temp.gameObject.AddComponent<AudioSource>();

            if (playSoundOnTriggerEnter.boolValue)
            {
                GUILayout.Label("Music Trigger Enter Configurations", EditorStyles.largeLabel);
                EditorGUILayout.PropertyField(musicConfigOnTriggerEnter, new GUIContent("Music Sounds Cofigurations"));
            }
            if (playSoundOnTriggerExit.boolValue)
            {
                GUILayout.Label("Music Trigger Exit Configurations", EditorStyles.largeLabel);
                EditorGUILayout.PropertyField(musicConfigOnTriggerExit, new GUIContent("Music Sounds Cofigurations"));
            }
            if (playSoundOnCollisionEnter.boolValue)
            {
                GUILayout.Label("Music Collision Enter Configurations", EditorStyles.largeLabel);
                EditorGUILayout.PropertyField(musicConfigOnCollisionEnter, new GUIContent("Music Sounds Cofigurations"));
            }
            if (playSoundOnStart.boolValue)
            {
                GUILayout.Label("Music On Start Configurations", EditorStyles.largeLabel);
                EditorGUILayout.PropertyField(musicConfigOnStart, new GUIContent("Music Sounds Cofigurations"));
            }
        }

        serializedObject.ApplyModifiedProperties();
    }  

    public void OnEnable()
    {
        //general variables
        soundType = serializedObject.FindProperty("_soundType");
        sfxSoundInterval = serializedObject.FindProperty("_sfxSoundInterval");
        soundBasedOnVelocity = serializedObject.FindProperty("_soundBasedOnVelocity");
        //baseAudioSourceRangeMultiplyer = serializedObject.FindProperty("_baseAudioSourceRangeMultiplyer");
        //trigger enter
        playSoundOnTriggerEnter = serializedObject.FindProperty("_playSoundOnTriggerEnter");
        musicConfigOnTriggerEnter = serializedObject.FindProperty("_musicConfigOnTriggerEnter");
        sfxConfigOnTriggerEnter = serializedObject.FindProperty("_sfxConfigOnTriggerEnter");
        //trigger exit
        playSoundOnTriggerExit = serializedObject.FindProperty("_playSoundOnTriggerExit");
        musicConfigOnTriggerExit = serializedObject.FindProperty("_musicConfigOnTriggerExit");
        sfxConfigOnTriggerExit = serializedObject.FindProperty("_sfxConfigOnTriggerExit");
        //collision enter
        playSoundOnCollisionEnter = serializedObject.FindProperty("_playSoundOnCollisionEnter");
        musicConfigOnCollisionEnter = serializedObject.FindProperty("_musicConfigOnCollisionEnter");
        sfxConfigOnCollisionEnter = serializedObject.FindProperty("_sfxConfigOnCollisionEnter");
        //on start
        playSoundOnStart = serializedObject.FindProperty("_playSoundOnStart");
        musicConfigOnStart = serializedObject.FindProperty("_musicConfigOnStart");
        sfxConfigOnStart = serializedObject.FindProperty("_sfxConfigOnStart");
    }    
}
#endif
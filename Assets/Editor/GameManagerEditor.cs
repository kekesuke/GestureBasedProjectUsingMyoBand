// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEditor;
// using UnityEditor.AnimatedValues;


// [CustomEditor(typeof(GamePlayController))]
// public class GameManagerEditor : Editor
// {
//     [SerializeField] Sprite rock;
//     [SerializeField] Sprite paper;
//     [SerializeField] Sprite scissors;

//     [SerializeField] Image playerChoiceImage;
//     [SerializeField] Image botChoiceImage;
//     [SerializeField] Text infoText;

//     AnimBool useGameManager;

//     private void OnEnable()
//     {
//         useGameManager = new AnimBool(false);
//         useGameManager.valueChanged.AddListener(Repaint);

//     }

//     public override void OnInspectorGUI()
//     {
//         useGameManager.target = EditorGUILayout.ToggleLeft("Turn on Manager", useGameManager.target);

//         if (EditorGUILayout.BeginFadeGroup(useGameManager.faded))
//         {
//             EditorGUI.indentLevel++;
//             Rect rect = EditorGUILayout.BeginVertical(GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
//             rock = (Sprite)EditorGUI.ObjectField(rect, "Rock", rock, typeof(Sprite), false);
//             EditorGUILayout.Space();
//             EditorGUILayout.EndVertical();

//             rect = EditorGUILayout.BeginVertical(GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
//             paper = (Sprite)EditorGUI.ObjectField(rect, "Paper", paper, typeof(Sprite), false);
//             EditorGUILayout.Space();
//             EditorGUILayout.EndVertical();

//             rect = EditorGUILayout.BeginVertical(GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
//             scissors = (Sprite)EditorGUI.ObjectField(rect, "Scissors", scissors, typeof(Sprite), false);
//             EditorGUILayout.Space();
//             EditorGUILayout.EndVertical();

//             rect = EditorGUILayout.BeginVertical(GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
//             playerChoiceImage = (Image)EditorGUI.ObjectField(rect, "playerChoiceImage", playerChoiceImage, typeof(Image), false);
//             EditorGUILayout.Space();
//             EditorGUILayout.EndVertical();

//             rect = EditorGUILayout.BeginVertical(GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
//             botChoiceImage = (Image)EditorGUI.ObjectField(rect, "botChoiceImage", botChoiceImage, typeof(Image), false);
//             EditorGUILayout.Space();
//             EditorGUILayout.EndVertical();

//             rect = EditorGUILayout.BeginVertical(GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
//             infoText = (Text)EditorGUI.ObjectField(rect, "infoText", botChoiceImage, typeof(Text), false);
//             EditorGUILayout.Space();
//             EditorGUILayout.EndVertical();

//             EditorGUI.indentLevel--;

//         }
//         EditorGUILayout.EndFadeGroup();
//     }
// }

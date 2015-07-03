using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AnimationGenerator))]
public class AnimationGeneratorUI : Editor {
	AnimationGenerator animatorGenerator;
	
	
	void OnEnable () {
		animatorGenerator = (AnimationGenerator)target;
	}
	
	public override void OnInspectorGUI () {
		DrawDefaultInspector();
		
		if (GUILayout.Button("Generate Frames", GUILayout.Height(50))) {
			animatorGenerator.GenerateFrames();
		}
		
		if (GUILayout.Button("Auto Names", GUILayout.Height(50))) {
			animatorGenerator.AutoName();
		}
		if (GUILayout.Button("Clean Names", GUILayout.Height(25))) {
			string autoName = animatorGenerator.autoName;
			animatorGenerator.autoName = "";
			animatorGenerator.AutoName();
			animatorGenerator.autoName = autoName;
		}
		if (GUILayout.Button("Toggle ONE", GUILayout.Height(25))) {
			for (int i=0; i<animatorGenerator.isOneFrame.Length; i++) {
				animatorGenerator.isOneFrame[i] = !animatorGenerator.isOneFrame[i];
			}
		}
		if (GUILayout.Button("Generate Animations", GUILayout.Height(50))) {
			animatorGenerator.GenerateAnimations();
		}
		if (GUILayout.Button("Save Animations", GUILayout.Height(50))) {
			for (int i=0; i<animatorGenerator.animationClips.Length; i++) {
				AssetDatabase.CreateAsset(
					animatorGenerator.animationClips[i],
					"Assets/Animations/" + animatorGenerator.animationClips[i].name + ".anim");
				AssetDatabase.SaveAssets();
			}
		}
		
		if (animatorGenerator.frameTextures.Length == 0) {
			return;
		}
		
		for (int i=0; i<animatorGenerator.sortedFrames.Length / 3; i++) {
			if (animatorGenerator.frameTextures[i * 3]) {
				if (i % 4 == 0) {
					EditorGUILayout.Space();
				}
				
				EditorGUILayout.BeginHorizontal();
				
				animatorGenerator.animationNames[i] = GUILayout.TextField(animatorGenerator.animationNames[i], GUILayout.MinWidth(100));
				
				animatorGenerator.isOneFrame[i] = GUILayout.Toggle(
					animatorGenerator.isOneFrame[i],
					"ONE");
				
				for (int j=0; j<3; j++) {
					GUILayout.Button(animatorGenerator.frameTextures[i * 3 + j], animatorGenerator.guiSkin.GetStyle("Button"));
				}
				
				EditorGUILayout.EndHorizontal();
			}
		}
	}
}

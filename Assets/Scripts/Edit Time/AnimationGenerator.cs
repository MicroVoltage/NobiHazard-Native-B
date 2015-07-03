using UnityEngine;
using UnityEditor;
using System.Collections;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class AnimationGenerator : MonoBehaviour {
	// Sprite sheet to use;
	public Texture2D spriteSheet;
	// Frame rate
	public int frameRate = 8;
	// Name for each animation null -> no animation
	public string[] animationNames = new string[4 * 16];
	// Is one frame? false -> four frames
	public bool[] isOneFrame = new bool[4 * 16];
	
	public string autoName;
	public string[] autoOrientation = new string[4];
	
	public GUISkin guiSkin;
	
	// Generated sprites
	public Sprite[] frames;
	// Sorted sprites
	public Sprite[] sortedFrames;
	// Frame Textures
	public Texture2D[] frameTextures;
	// Animations
	public AnimationClip[] animationClips;
	
	private Animator animator;
	
	
	public void AutoName () {
		if (autoName == "") {
			for (int i=0; i<sortedFrames.Length/3; i++) {
				for (int j=0; j<4; j++) {
					if (i * 4 + j < animationNames.Length) {
						animationNames[i * 4 + j] = "";
					}
				}
			}
			
			return;
		}
		
		for (int i=0; i<sortedFrames.Length/3; i++) {
			if (animationNames[i * 4] != "" && autoName[0] != animationNames[i * 4][0]){
				string gName = animationNames[i * 4];
				for (int j=0; j<4; j++) {
					animationNames[i * 4 + j] = autoName + "-" + gName + "-" + autoOrientation[j];
				}
			}
		}
	}
	
	public void GenerateAnimations () {
		animationClips = new AnimationClip[sortedFrames.Length / 3];
		
		int animationCount = 0;
		for (int i=0; i<animationClips.Length; i++) {
			if (animationNames[i] != "") {
				AnimationClip animationClip;
				if (isOneFrame[i]) {
					animationClip = GenerateAnimation(animationNames[i], frameRate, i * 3 + 1, 1); // use 2nd sprite
				} else {
					animationClip = GenerateAnimation(animationNames[i], frameRate, i * 3, 4);
					
					SerializedObject serializedClip = new SerializedObject(animationClip);
					AnimationClipSettings clipSettings = new AnimationClipSettings(serializedClip.FindProperty("m_AnimationClipSettings"));
					
					clipSettings.loopTime = true;
					
					serializedClip.ApplyModifiedProperties();
				}
				
				animationClips[animationCount] = animationClip;
				animationCount++;
			}
		}
	}
	
	AnimationClip GenerateAnimation (string name, int frameRate, int frameNo, int frameCount) {
		AnimationClip animationClip = new AnimationClip();
		animationClip.name = name;
		animationClip.frameRate = frameRate;
//		AnimationUtility.SetAnimationType(animationClip, ModelImporterAnimationType.Generic);
		
		EditorCurveBinding curveBinding = new EditorCurveBinding();
		
		curveBinding.type = typeof(SpriteRenderer);
		curveBinding.path = "";
		curveBinding.propertyName = "m_Sprite";
		
		ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[frameCount];
		for (int i=0; i<frameCount; i++) {
			keyframes[i] = new ObjectReferenceKeyframe();
			keyframes[i].time = (float)i / frameRate;
			keyframes[i].value = sortedFrames[frameNo + i];
		}
		
		if (frameCount == 4) {
			keyframes[3].value = sortedFrames[frameNo + 1];
		}
		
		AnimationUtility.SetObjectReferenceCurve(animationClip, curveBinding, keyframes);
		
		Debug.Log("New animation generated.");
		return animationClip;
	}
	
	public void GenerateFrames () {
		// Import sprite sheet
		string spriteSheetPath = AssetDatabase.GetAssetPath(spriteSheet);
		TextureImporter spriteSheetAsset = (TextureImporter)AssetImporter.GetAtPath(spriteSheetPath);
		// Set to readable
		if (!spriteSheetAsset.isReadable) {
			spriteSheetAsset.isReadable = true;
			AssetDatabase.ImportAsset (spriteSheetPath);
		}
		// Load sprites
		Object[] spriteSheetObjects = AssetDatabase.LoadAllAssetsAtPath(spriteSheetPath);
		
		frames = new Sprite[spriteSheetObjects.Length];
		for (int i=1; i<spriteSheetObjects.Length; i++) {
			frames[i] = (Sprite)spriteSheetObjects[i];
		}
		
		SortFrames();
	}
	
	void SortFrames () {
		sortedFrames = new Sprite[frames.Length];
		//		animationNames = new string[4 * 16];
		//		isOneFrame = new bool[4 * 16];
		frameTextures = new Texture2D[frames.Length];
		
		int g = 0;
		for (int i=0; i<4; i++) {
			int iAnchorY = 384 - i * 128;
			
			for (int j=0; j<4; j++) {
				int jAnchorX = j * 72;
				
				for (int k=0; k<4; k++) {
					int kAnchorY = 96 - k * 32;
					
					bool hasFrame = false;
					for (int l=0; l<3; l++) {
						int lAnchorX = l * 24;
						
						int frameNo = FindFrameAtPosition(jAnchorX + lAnchorX, iAnchorY + kAnchorY);
						if (frameNo > 0) {
							sortedFrames[g * 3 + l] = frames[frameNo];
							
							Rect rect = frames[frameNo].rect;
							frameTextures[g * 3 + l] = new Texture2D((int)rect.width, (int)rect.height);
							frameTextures[g * 3 + l].SetPixels(spriteSheet.GetPixels(
								(int)rect.x,
								(int)rect.y,
								(int)rect.width,
								(int)rect.height));
							frameTextures[g * 3 + l].Apply();
							
							hasFrame = true;
						}
					}
					if (hasFrame) {
						g++;
					}
				}
			}
		}
	}
	
	int FindFrameAtPosition (float x, float y) {
		for (int i=1; i<frames.Length; i++) {
			Rect rect = frames[i].rect;
			Vector2 tilePositions = new Vector2(rect.x, rect.y);
			if ((int)tilePositions.x == (int)x && (int)tilePositions.y == (int)y) {
				return i;
			}
		}
		return 0;
	}
}

class AnimationClipSettings
{
	SerializedProperty m_Property;
	
	private SerializedProperty Get (string property)
	{
		return m_Property.FindPropertyRelative (property);
	}
	
	public AnimationClipSettings (SerializedProperty prop)
	{
		m_Property = prop;
	}
	
	public float startTime   { get { return Get ("m_StartTime").floatValue; } set { Get ("m_StartTime").floatValue = value; } }
	public float stopTime	{ get { return Get ("m_StopTime").floatValue; } set { Get ("m_StopTime").floatValue = value; } }
	public float orientationOffsetY { get { return Get ("m_OrientationOffsetY").floatValue; } set { Get ("m_OrientationOffsetY").floatValue = value; } }
	public float level { get { return Get ("m_Level").floatValue; } set { Get ("m_Level").floatValue = value; } }
	public float cycleOffset { get { return Get ("m_CycleOffset").floatValue; } set { Get ("m_CycleOffset").floatValue = value; } }
	
	public bool loopTime { get { return Get ("m_LoopTime").boolValue; } set { Get ("m_LoopTime").boolValue = value; } }
	public bool loopBlend { get { return Get ("m_LoopBlend").boolValue; } set { Get ("m_LoopBlend").boolValue = value; } }
	public bool loopBlendOrientation { get { return Get ("m_LoopBlendOrientation").boolValue; } set { Get ("m_LoopBlendOrientation").boolValue = value; } }
	public bool loopBlendPositionY { get { return Get ("m_LoopBlendPositionY").boolValue; } set { Get ("m_LoopBlendPositionY").boolValue = value; } }
	public bool loopBlendPositionXZ { get { return Get ("m_LoopBlendPositionXZ").boolValue; } set { Get ("m_LoopBlendPositionXZ").boolValue = value; } }
	public bool keepOriginalOrientation { get { return Get ("m_KeepOriginalOrientation").boolValue; } set { Get ("m_KeepOriginalOrientation").boolValue = value; } }
	public bool keepOriginalPositionY { get { return Get ("m_KeepOriginalPositionY").boolValue; } set { Get ("m_KeepOriginalPositionY").boolValue = value; } }
	public bool keepOriginalPositionXZ { get { return Get ("m_KeepOriginalPositionXZ").boolValue; } set { Get ("m_KeepOriginalPositionXZ").boolValue = value; } }
	public bool heightFromFeet { get { return Get ("m_HeightFromFeet").boolValue; } set { Get ("m_HeightFromFeet").boolValue = value; } }
	public bool mirror { get { return Get ("m_Mirror").boolValue; } set { Get ("m_Mirror").boolValue = value; } }
}



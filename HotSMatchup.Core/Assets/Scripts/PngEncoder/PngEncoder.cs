using UnityEngine;
using System.Collections;
using System.IO;

namespace Encoder
{
	public class PngEncoder : MonoBehaviour {

		public static bool IsEncoding = false;

		//This is the main function to call
		public static void CaptureToPng (int width, int height, Camera camera, string filename){

			if(IsEncoding) {
				return;
			}
			var inst = GetInstance ();
			inst.StartEncoder (width, height, camera, filename);
		}

		static PngEncoder GetInstance ()
		{
			var inst = GameObject.FindObjectOfType<PngEncoder> ();
			if (inst != null) {
				return inst;
			}
			var go = new GameObject ("_PngEncoder");
			inst = go.AddComponent<PngEncoder> ();
			return inst;
		}

		void StartEncoder (int width, int height, Camera camera, string filename) {

			StartCoroutine(EncoderCoroutine(width, height, camera, filename));
		}

		IEnumerator EncoderCoroutine (int width, int height, Camera camera, string filename) {

			IsEncoding = true;

			//Create Render Texture and draw to it from the Photo Camera
			RenderTexture renderTex = new RenderTexture (width, height, 24);
			camera.targetTexture = renderTex;
			RenderTexture.active = renderTex;
			camera.Render();
			
			//Create Texture2D from Render Texture
			Texture2D tex2d = new Texture2D (width, height, TextureFormat.RGB24, false);
			tex2d.ReadPixels(new Rect(0, 0, width, height), 0, 0);
			tex2d.Apply();
			
			//Encode the Texture2D to PNG and destroy the Texture2D
			byte[] bytes = tex2d.EncodeToPNG();
			//JPGEncoder encoder = new JPGEncoder(tex2d, 90, filename);

			Destroy (tex2d);
			
			File.WriteAllBytes(filename, bytes);
			
			camera.targetTexture = null;
			RenderTexture.active = null; // added to avoid errors 
			DestroyImmediate(renderTex);

			IsEncoding = false;

			yield return null;
		}
	}
}

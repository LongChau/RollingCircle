using DG.Tweening;
using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LC.Ultility
{
    /// <summary>
    /// All global functions in here.
    /// </summary>
    public static class GlobalFunc
    {
        // This script loads a PNG or JPEG image from disk and returns it as a Sprite
        // Drop it on any GameObject/Camera in your scene (singleton implementation)
        //
        // Usage from any other script:
        // MySprite = GlobalFunc.instance.LoadNewSprite(FilePath, [PixelsPerUnit (optional)])

        //private static GlobalFunc _instance;

        //public static GlobalFunc instance
        //{
        //    get
        //    {
        //        //If _instance hasn't been set yet, we grab it from the scene!
        //        //This will only happen the first time this reference is used.

        //        if (_instance == null)
        //            _instance = GameObject.FindObjectOfType<GlobalFunc>();
        //        return _instance;
        //    }
        //}

        public static Sprite LoadNewSprite(string FilePath, Vector2 pivot, float PixelsPerUnit = 100.0f)
        {
            // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
            Sprite NewSprite;
            Texture2D SpriteTexture = LoadTexture(FilePath);
            NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), pivot, PixelsPerUnit);

            return NewSprite;
        }

        public static Texture2D LoadTexture(string FilePath)
        {
            // Load a PNG or JPG file from disk to a Texture2D
            // Returns null if load fails

            Texture2D Tex2D;
            byte[] FileData;

            if (File.Exists(FilePath))
            {
                FileData = File.ReadAllBytes(FilePath);
                Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
                if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                    return Tex2D;                 // If data = readable -> return texture
            }
            return null;                     // Return null if load failed
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="gameObj" gameObj can scale></param>
        /// <param name="scale" scale x,y,z tu dat></param>
        /// <param name="step" buoc nhay></param>
        public static void ScaleUp(GameObject gameObj, ref Vector3 scale, float step)
        {
            scale.x += step;
            scale.y += step;
            scale.z += step;
            if (scale.x >= 1)
            {
                scale.x = 1;
            }

            if (scale.y >= 1)
            {
                scale.y = 1;
            }

            if (scale.z >= 1)
            {
                scale.z = 1;
            }

            gameObj.transform.localScale = scale;
        }

        public static string ConvertToTimeString_MM_SS(int nTime)
        {
            string nTimeString = "";
            int minutes = Mathf.FloorToInt(nTime / 60F);
            int seconds = Mathf.FloorToInt(nTime - minutes * 60);
            nTimeString = string.Format("{0:00}:{1:00}", minutes, seconds);
            return nTimeString;
        }

        public static void SetVisible(GameObject gameObj, bool bVisible)
        {
            if (bVisible == true)
            {
                gameObj.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (bVisible == false)
            {
                gameObj.transform.localScale = new Vector3(0, 0, 0);
            }
        }

        public static bool AnimContainsParam(this Animator _Anim, string _ParamName)
        {
            foreach (AnimatorControllerParameter param in _Anim.parameters)
            {
                if (param.name == _ParamName) return true;
            }
            return false;
        }

        /// <summary>
        /// make gameobject jump up then down
        /// </summary>
        public static void JumpUpThenDown(Transform transform)
        {
            Transform tempTransform = transform;
            //transform.position = Vector3.MoveTowards(transform.position, playerBag.position, 0.25f);
        }

        //public static void JumpUp(Transform transform, float step)
        //{
        //    Debug.Log("JumpUp");
        //    transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 2, 0), step);
        //}

        public static IEnumerator JumpUp(Transform transform, float height, float step)
        {
            float tempHeight = transform.position.y + height;
            //float tempHeight = height;
            Vector3 tempPos = transform.position;
            for (float f = 0; f < height; f++)
            {
                tempPos.y += step;
                transform.position = tempPos;
                yield return null;
            }
        }

        //public static IEnumerator Fade()
        //{
        //    for (float f = 1f; f >= 0; f -= 0.1f)
        //    {
        //        Color c = renderer.material.color;
        //        c.a = f;
        //        renderer.material.color = c;
        //        yield return null;
        //    }
        //}

        public static IEnumerator FadeOut(Color c)
        {
            float a = 1;
            for (int i = 0; i < 10; i++)
            {
                a -= 0.1f;
                c = new Color(1f, 1f, 1f, a);
                yield return null;
            }
        }

        public static IEnumerator FadeIn(Color c)
        {
            float a = 0;
            for (int i = 0; i < 10; i++)
            {
                a += 0.1f;
                c = new Color(1f, 1f, 1f, a);
                yield return null;
            }
        }

        public static IEnumerator ScaleUpAction(GameObject gameObj, float step, float scaleValue)
        {
            gameObj.transform.localScale = new Vector3(0, 0, 0);
            Vector3 scaleTemp = new Vector3(0, 0, 0);
            for (int i = 0; i < scaleValue; i++)
            {
                scaleTemp.x += step;
                scaleTemp.y += step;
                scaleTemp.z += step;
                gameObj.transform.localScale = scaleTemp;
                yield return null;
            }
        }

        public static IEnumerator ScaleDownAction(GameObject gameObj, float step, float scaleValue)
        {
            //gameObj.transform.localScale = new Vector3(0, 0, 0);
            Vector3 scaleTemp = gameObj.transform.localScale;
            for (int i = 0; i < scaleValue; i++)
            {
                scaleTemp.x -= step;
                scaleTemp.y -= step;
                scaleTemp.z -= step;
                gameObj.transform.localScale = scaleTemp;
                yield return null;
            }
        }

        public static IEnumerator ShowPopupAction(GameObject gameObj)
        {
            float step = 0.1f;
            //Vector3 scaleTemp2 = gameObj.transform.localScale;
            gameObj.transform.localScale = new Vector3(0, 0, 0);
            Vector3 scaleTemp1 = new Vector3(0, 0, 0);
            for (int i = 0; i < 12; i++)    // to 12
            {
                scaleTemp1.x += step;
                scaleTemp1.y += step;
                scaleTemp1.z += step;
                gameObj.transform.localScale = scaleTemp1;
                yield return null;
            }
            step = 0.05f;
            for (int i = 0; i < 8; i++) // 12 - 4 = 8
            {
                scaleTemp1.x -= step;
                scaleTemp1.y -= step;
                scaleTemp1.z -= step;
                gameObj.transform.localScale = scaleTemp1;
                yield return null;
            }
            //step = 0.1f;
            for (int i = 0; i < 4; i++) // 10
            {
                scaleTemp1.x += step;
                scaleTemp1.y += step;
                scaleTemp1.z += step;
                gameObj.transform.localScale = scaleTemp1;
                yield return null;
            }
        }

        public static void TweenShowPopup(Transform target, float time = 0.2f)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(target.DOScale(new Vector3(1.2f, 1.2f, 1.0f), time));
            sequence.Append(target.DOScale(new Vector3(0.9f, 0.9f, 1.0f), time));
            sequence.Append(target.DOScale(new Vector3(1.0f, 1.0f, 1.0f), time));
        }

        public static void TweenScaleBouncingUp(Transform target, float time)
        {
        }

        public static void TweenScaleBouncingDown(Transform target, float time)
        {
        }

        public static void TweenScaleChewingGum(Transform target, float time)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(target.DOScale(new Vector3(1.2f, 1.2f, 1.0f), time));
            sequence.Append(target.DOScale(new Vector3(0.9f, 0.9f, 1.0f), time));

            sequence.Append(target.DOScale(new Vector3(0.9f, 1.2f, 1.0f), time));
            sequence.Append(target.DOScale(new Vector3(1.2f, 0.9f, 1.0f), time));
            sequence.Append(target.DOScale(new Vector3(1.0f, 1.0f, 1.0f), time));
        }

        public static void TweenMovingObjectLoop(Transform target, Vector3 startPoint, Vector3 endPoint, float delayTime, float timeComplete)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(delayTime);
            sequence.AppendCallback(() =>
            {
                target.DOMove(endPoint, timeComplete).OnComplete(() =>
                {
                    target.position = startPoint;
                });
            });

            sequence.SetLoops(-1, LoopType.Restart);
        }

        public static void FlipTargetObject(Transform target, bool isFacingRight)
        {
            Vector3 tempTargetScale = target.localScale;

            if (isFacingRight)
            {
                tempTargetScale.x = -1;
            }
            else
            {
                tempTargetScale.x = 1;
            }

            target.localScale = tempTargetScale;
        }

        /// <summary>
        /// Greater or lower image opacity
        /// </summary>
        /// <param name="target">target object</param>
        /// <param name="opacityValue">opacity value. (0.0f <= value <= 1.0f)</param>
        public static void SetObjectImageOpacity(Transform target, float opacityValue)
        {
            Color designColor = new Color(1, 1, 1, opacityValue);

            target.GetComponent<Image>().color = designColor;
        }

        public static void SetImageRGBColor(Image target,
            float red, float green, float blue)
        {
            Color designColor = new Color(red / 255, green / 255, blue / 255);
            target.color = designColor;
        }

        public static void SetImageRGBColor(Image target,
            float red, float green, float blue, float opacity)
        {
            Color designColor = new Color(red / 255, green / 255, blue / 255, opacity / 255);
            target.color = designColor;
        }

        public static void CountDownTextEffect(TextMeshProUGUI nText, Color nColor)
        {
            Color defaultColor = new Color(255 / 255, 255 / 255, 255 / 255, 255 / 255);
            nText.color = nColor;

            Color tempColor = nText.color;
            Color hideColor = tempColor;
            hideColor.a = 0;

            nText.DOColor(hideColor, 0.8f).SetLoops(-1);
        }

        public static string GenerateUUID()
        {
            Guid g = Guid.NewGuid();
            string uuid = g.ToString();
            return uuid;
        }

        public static Sprite ConvertTextureToSprite(Texture2D texture)
        {
            Texture2D avatar = texture;
            Sprite sprite = Sprite.Create(avatar, new Rect(0, 0, avatar.width, avatar.height), new Vector2());
            return sprite;
        }

        public static void RotateAround(Transform transform, Vector3 rotateVec)
        {
            transform.Rotate(rotateVec);
        }
    }
}
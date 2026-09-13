using Unity.Cinemachine;
using UnityEngine;

namespace Sabanishi.MebuMekaFarm.MainGame
{
    public class CameraPropInitializer : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinemachineCamera;
        [SerializeField] private Camera targetCamera;

        public void Initialize(int width, int height)
        {
            float x = (float)(width - 1) / 2;
            float y = (float)(height - 1) / 2;
            Debug.Log(x + " " + y);
            cinemachineCamera.transform.position = new Vector3(x, y, -10);
            Debug.Log($"world={cinemachineCamera.transform.position}");
            Debug.Log($"local={cinemachineCamera.transform.localPosition}");
            Debug.Log($"parent={cinemachineCamera.transform.parent}");

            float aspect = targetCamera.aspect;
            cinemachineCamera.Lens.OrthographicSize = CalcOrthographicSize(width, height, aspect);
        }

        private float CalcOrthographicSize(float width, float height, float aspectRatio)
        {
            return Mathf.Max(height / 2f, width / (2f * aspectRatio));
        }
    }
}
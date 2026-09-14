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
            cinemachineCamera.transform.position = new Vector3(x, y, -10);

            float aspect = targetCamera.aspect;
            cinemachineCamera.Lens.OrthographicSize = CalcOrthographicSize(width, height, aspect);
        }

        private float CalcOrthographicSize(float width, float height, float aspectRatio)
        {
            return Mathf.Min(height / 2f, width / (2f * aspectRatio));
        }
    }
}
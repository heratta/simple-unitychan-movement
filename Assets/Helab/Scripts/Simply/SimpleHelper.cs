using UnityEngine;

namespace Helab.Simply
{
    public static class SimpleHelper
    {
        private const float ValidThresholdsForJoystick = 0.1f;
        
        public static (Vector3, float) GetThrustDirectionByInput()
        {
            var dir = Vector3.zero;
            dir += GetDirectionByKeyboard(KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D);
            dir += GetDirectionByJoystick("Horizontal_L", "Vertical_L");

            return (dir.normalized, dir.magnitude);
        }

        public static Vector3 GetCameraRotateValueByInput()
        {
            var dir = Vector3.zero;
            dir += GetDirectionByKeyboard(
                KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow);
            dir += GetDirectionByJoystick("Horizontal_R", "Vertical_R");

            return dir.normalized;
        }

        private static Vector3 GetDirectionByKeyboard(KeyCode forward, KeyCode back, KeyCode left, KeyCode right)
        {
            var dir = Vector3.zero;

            if (Input.GetKey(forward))
            {
                dir += Vector3.forward;
            }

            if (Input.GetKey(back))
            {
                dir += Vector3.back;
            }

            if (Input.GetKey(left))
            {
                dir += Vector3.left;
            }

            if (Input.GetKey(right))
            {
                dir += Vector3.right;
            }

            return dir;
        }

        private static Vector3 GetDirectionByJoystick(string xAxisName, string yAxisName)
        {
            var dir = Vector3.zero;

            if (!string.IsNullOrEmpty(xAxisName))
            {
                float xAxis = Input.GetAxis(xAxisName);
                if (ValidThresholdsForJoystick < Mathf.Abs(xAxis))
                {
                    dir += new Vector3(xAxis, 0f, 0f);
                }
            }

            if (!string.IsNullOrEmpty(yAxisName))
            {
                float yAxis = Input.GetAxis(yAxisName);
                if (ValidThresholdsForJoystick < Mathf.Abs(yAxis))
                {
                    dir += new Vector3(0f, 0f, yAxis);
                }
            }

            return dir;
        }
    }
}

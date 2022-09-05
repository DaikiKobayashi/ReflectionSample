using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SubCamera : MonoBehaviour
{
    [SerializeField] Camera m_Camera;

    private Vector4 reflectionPlane = new Vector4(0f, 1f, 0f, 0f);

    private void calc_reflection_matrix(ref Matrix4x4 mat, Vector4 plane)
    {
        mat.m00 = (1f - 2f * plane.x * plane.x);
        mat.m01 = (-2f * plane.x * plane.y);
        mat.m02 = (-2f * plane.x * plane.z);
        mat.m03 = (-2f * plane.x * plane.w);
        mat.m10 = (-2f * plane.y * plane.x);
        mat.m11 = (1f - 2f * plane.y * plane.y);
        mat.m12 = (-2f * plane.y * plane.z);
        mat.m13 = (-2f * plane.y * plane.w);
        mat.m20 = (-2f * plane.z * plane.x);
        mat.m21 = (-2f * plane.z * plane.y);
        mat.m22 = (1f - 2f * plane.z * plane.z);
        mat.m23 = (-2f * plane.z * plane.w);
        mat.m30 = 0f;
        mat.m31 = 0f;
        mat.m32 = 0f;
        mat.m33 = 1f;
    }

    public void Update()
    {
        var reflection_matrix = new Matrix4x4();
        calc_reflection_matrix(ref reflection_matrix, reflectionPlane);

        var local_reflection_matrix = m_Camera.worldToCameraMatrix * reflection_matrix;
        {
            var normal = new Vector3(reflectionPlane.x, reflectionPlane.y, reflectionPlane.z);
            Vector3 cnormal = local_reflection_matrix.MultiplyVector(normal);
            Vector3 cpos = local_reflection_matrix.MultiplyPoint(Vector3.zero);
            Vector4 clip_plane = new Vector4(cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos, cnormal));
            m_Camera.worldToCameraMatrix = local_reflection_matrix;
            m_Camera.projectionMatrix = m_Camera.CalculateObliqueMatrix(clip_plane);
        } 
    }
}

using System.Collections;
using UnityEngine;

public class HitVFX : MonoBehaviour
{
    [SerializeField]
    private Renderer[] renderers;

    [SerializeField]
    private Material flashMaterial;

    [SerializeField]
    private float flashDuration = 0.1f;

    private Material[][] originalMaterials;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        originalMaterials = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].sharedMaterials;
        }
    }

    public void Flash()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        foreach (Renderer renderer in renderers)
        {
            Material[] flashMaterials = new Material[renderer.sharedMaterials.Length];

            for (int i = 0; i < flashMaterials.Length; i++)
            {
                flashMaterials[i] = flashMaterial;
            }

            renderer.sharedMaterials = flashMaterials;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].sharedMaterials = originalMaterials[i];
        }

        flashCoroutine = null;
    }
}

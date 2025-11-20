using UnityEngine;

public class DragAndThrow : MonoBehaviour
{
    private Rigidbody rb;
    //private bool isDragging = false;
    private Vector3 lastMousePosition;
    public Vector3 throwVelocity;

    private float startTime;
    protected float endTime;
    public float time;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        //rb.constraints = RigidbodyConstraints.FreezePosition;
    }

    void OnMouseDown()
    {
        startTime = Time.time;
        //rb.constraints = RigidbodyConstraints.None;
        //isDragging = true;
        rb.linearVelocity = Vector3.zero;
        lastMousePosition = Input.mousePosition;
    }

    void OnMouseDrag()
    {
        /*
        if (!isDragging) return;

        // Obtenir la position actuelle de la souris
        Vector3 mousePos = Input.mousePosition;
        
        // Calculer la vitesse de déplacement de la souris
        throwVelocity = (mousePos - lastMousePosition) * 0.1f;
        lastMousePosition = mousePos;

        // Convertir la position de la souris en position 3D dans le monde
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.WorldToScreenPoint(transform.position).z));
        //transform.position = worldPos;
        */
    }

    void OnMouseUp()
    {
        endTime = Time.time;
        Vector3 mousePos = Input.mousePosition;
        throwVelocity = (mousePos - lastMousePosition) * 0.1f;

        time = (endTime - startTime) * 20;
        throwVelocity = throwVelocity / time;

        rb.constraints = RigidbodyConstraints.None;
        //isDragging = false;
        rb.useGravity = true;

        // Définir la direction du lancer en fonction de la caméra
        Vector3 throwDirection = Camera.main.transform.forward.normalized;

        // Appliquer une force dans la direction de la caméra
        rb.linearVelocity = throwDirection * Mathf.Abs(throwVelocity.y) * 5f + new Vector3(throwVelocity.x, throwVelocity.y, 0);

        // Ajuster la puissance du lancer selon tes besoins
    }
}

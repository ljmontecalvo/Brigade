using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public LineRenderer bulletRayVisual;
    public Transform firePoint;
    public LayerMask fireContacts;
    public Transform blaster;
    public float movementSpeed = 4f;
    public float fireRange;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        bulletRayVisual.enabled = false;
    }

    private void Update()
    {
        if (!_camera) return;

        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");
        var fire = Input.GetButtonDown("Fire1");
        var mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        if (fire)
        {
            Fire(mousePos);
        }

        transform.position += new Vector3(x, y, 0) * (Time.deltaTime * movementSpeed);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
        blaster.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - blaster.position);
    }

    private void Fire(Vector3 mousePosition)
    {
        var hit = Physics2D.Raycast(firePoint.position, transform.up, fireRange, fireContacts);
        

        if (hit.collider)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }
            
            bulletRayVisual.SetPosition(1, new Vector3(0, hit.point.y, 0));
            Debug.DrawRay(firePoint.position, transform.up * (Vector3.Distance(firePoint.position, hit.point)), Color.red, 2f);
        }
        else
        {
            bulletRayVisual.SetPosition(1, new Vector3(0, fireRange, 0));
            Debug.DrawRay(firePoint.position, transform.up * fireRange, Color.red, 2f);
        }

        StartCoroutine(ShowRay());
    }

    private IEnumerator ShowRay()
    {
        bulletRayVisual.enabled = true;

        yield return new WaitForSeconds(0.1f);

        bulletRayVisual.enabled = false;
    }
}

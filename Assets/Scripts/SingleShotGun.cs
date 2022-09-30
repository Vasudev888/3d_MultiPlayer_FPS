using Photon.Pun;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] Camera playerCam;
    PhotonView pV;

    private void Awake()
    {
        pV = GetComponent<PhotonView>();
    }

    public override void Use()
    {
        Shoot();
        
    }

    void Shoot()
    {
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = playerCam.transform.position;

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamagable>()?.TakeDamage(((GunInfo)itemInfo).Damage);
            pV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPos, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPos, 0.3f);
        if(colliders.Length != 0)
        {

            GameObject bulletImpactObj =  Instantiate(bulletImpactPrefab, hitPos + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 10f);

            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }

    }
}

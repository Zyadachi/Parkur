using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab pocisku
    public Transform bulletSpawnPoint; // Punkt początkowy dla pocisków
    public float bulletSpeed = 10f; // Prędkość pocisku
    public float fireRate = 0.5f; // Czas między kolejnymi strzałami

    private float fireTimer; // Licznik czasu do kolejnego strzału

    private void Update()
    {
        // Zaktualizuj licznik czasu
        fireTimer += Time.deltaTime;

        // Sprawdź, czy gracz naciska przycisk strzału i czy minął wymagany czas między strzałami
        if (Input.GetButtonDown("Fire1") && fireTimer >= fireRate)
        {
            // Wystrzel pocisk
            Fire();
            // Zresetuj licznik czasu
            fireTimer = 0f;
        }
    }

    private void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Pobierz aktualną rotację kamery gracza
        Quaternion cameraRotation = Camera.main.transform.rotation;

        // Ustaw rotację pocisku na rotację kamery gracza
        bullet.transform.rotation = cameraRotation;

        // Dodaj siłę do pocisku, aby nadawać mu prędkość
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
    }
}

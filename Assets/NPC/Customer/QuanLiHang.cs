using UnityEngine;
using System.Collections; // Cần thêm dòng này để dùng Coroutine
using System.Collections.Generic;

public class QueueManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public List<Transform> queueSlots;
    public int totalCustomersInLevel = 20;

    [Header("Spawn Timing")]
    public float minSpawnDelay = 1f; 
    public float maxSpawnDelay = 3f; 

    private int customersSpawned = 0;
    private List<GameObject> customersInLine = new List<GameObject>();
    private bool isSpawning = false; // Biến kiểm soát để không chạy chồng chéo Coroutine


    void Start()
    {
        // Bắt đầu quá trình gọi khách từ từ
        StartCoroutine(SpawnRoutine());
    }

    // Coroutine quản lý việc hồi khách
    IEnumerator SpawnRoutine()
    {
        isSpawning = true;

        // Chừng nào chưa đủ tổng số khách của màn chơi
        while (customersSpawned < totalCustomersInLevel)
        {
            // Kiểm tra xem hàng đợi hiển thị còn chỗ trống không
            if (customersInLine.Count < queueSlots.Count)
            {
                // Đợi một khoảng thời gian ngẫu nhiên từ 1 đến 3 giây
                float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
                yield return new WaitForSeconds(randomDelay);

                // Sau khi đợi xong, kiểm tra lại một lần nữa trước khi tạo khách
                if (customersInLine.Count < queueSlots.Count)
                {
                    SpawnNewCustomer();
                }
            }
            else
            {
                // Nếu hàng đầy, đợi một chút rồi kiểm tra lại (để tránh treo máy)
                yield return new WaitForSeconds(0.5f);
            }
        }

        isSpawning = false;
    }

    void SpawnNewCustomer()
    {
        if (customersSpawned >= totalCustomersInLevel) return;

        customersSpawned++;
        GameObject newCustomer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        customersInLine.Add(newCustomer);

        UpdateQueuePositions();
        Debug.Log($"Khách mới đã vào hàng. Tổng đã xuất hiện: {customersSpawned}");
    }

    public void ServeCustomer()
    {
        if (customersInLine.Count > 0)
        {
            GameObject firstCustomer = customersInLine[0];
            customersInLine.RemoveAt(0);
            Destroy(firstCustomer);

            UpdateQueuePositions();

            // Lưu ý: Không cần gọi FillQueue nữa vì SpawnRoutine vẫn đang chạy ngầm 
            // để tự kiểm tra và lấp chỗ trống.
        }
    }

    void UpdateQueuePositions()
    {
        for (int i = 0; i < customersInLine.Count; i++)
        {
            Customer script = customersInLine[i].GetComponent<Customer>();
            if (script != null)
            {
                script.targetPosition = queueSlots[i].position;
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ServeCustomer();
        }
    }
}
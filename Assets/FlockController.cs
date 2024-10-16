using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FlockController : MonoBehaviour
{
    [SerializeField] private List<AnimalController> animalPrefabs;
    [SerializeField] private int initialAmount;
    [SerializeField] private float animalNeighborRadius = 3f;
    [SerializeField] private Transform mouseTarget;
    private int m_batchSize = 1;
    private List<AnimalController> animals = new();
    private float _assignTimer;

    private void Start()
    {
        for (int i = 0; i < initialAmount; i++) {
            var animal = Instantiate(animalPrefabs[Random.Range(0, animalPrefabs.Count)]);
            animal.SetTarget(transform);
            animals.Add(animal);
        }
    }

    Task TaskOnMainThread()
    {
        var tcs = new TaskCompletionSource<bool>();
        SynchronizationContext.Current.Post(_ => tcs.SetResult(true), null);
        return tcs.Task;
    }

    private void CheckAnimalNeighbors()
    {
        foreach (var animal in animals)
        {
            List<AnimalController> neighbors = new List<AnimalController>();
            foreach (var animal2 in animals)
            {
                if (Vector3.Distance(animal.transform.position, animal2.transform.position)<=animalNeighborRadius&&animal2.gameObject != animal.gameObject)
                {
                    neighbors.Add(animal2);
                }
            }
            animal.SetNeighbors(neighbors);
        }
    }

    private void Update()
    {
        if(_assignTimer>0)
            _assignTimer -= Time.deltaTime;
        CheckAnimalNeighbors();
        if(Input.GetMouseButtonDown(0)) {
            var animal = animals.Where(obj => obj.GetTarget().Equals(transform)).OrderBy(obj => (obj.transform.position - mouseTarget.position).sqrMagnitude).FirstOrDefault();
            animal.SetTarget(mouseTarget);
            animal.SetSpeed(3f);
        }
        if (Input.GetMouseButtonUp(0))
        {
            foreach (var animal in animals)
            {
                animal.SetTarget(transform);
                animal.SetSpeed(2f);
            }
        }
        if(Input.GetMouseButton(0)) {
            if (_assignTimer <= 0)
            {
                var freeAnimals = animals.Where(obj => obj.GetTarget().Equals(transform));
                if (freeAnimals.Count() > 0)
                {
                    var animal = freeAnimals.OrderBy(obj => (obj.transform.position - mouseTarget.position).sqrMagnitude).FirstOrDefault();
                    animal.SetTarget(mouseTarget);
                    animal.SetSpeed(3f);
                    _assignTimer = 0.2f;
                }
             }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.CompareTag("Ground"))
                    mouseTarget.position = hit.point;
            }
        }
    }
}

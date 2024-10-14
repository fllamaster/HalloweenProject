using UnityEngine;

public class GhostFollow : MonoBehaviour
{
    public Transform player;          // Ссылка на объект игрока
    public float speed = 5f;          // Скорость передвижения призрака
    public float stoppingDistance = 2f;   // Расстояние, на котором призрак остановится
    public int damage = 10;           // Количество урона, наносимое игроку
    public float headOffset = 1.5f;   // Смещение на уровень головы игрока

    private PlayerHealth playerHealth;  // Ссылка на компонент здоровья игрока
    private Animator animator;         // Ссылка на компонент Animator
    private bool hasAttacked = false;  // Флаг, чтобы предотвратить повторное нанесение урона

    void Start()
    {
        // Если игрок не установлен в инспекторе, найти его по тегу
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Находим компонент здоровья игрока
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }

        // Получаем компонент Animator
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Если призрак уже атаковал, не преследуем игрока
        if (hasAttacked) return;

        // Проверяем, находится ли игрок в сцене и есть ли у него компонент здоровья
        if (player != null && playerHealth != null)
        {
            // Создаем целевую позицию на уровне головы игрока
            Vector3 targetPosition = player.position + Vector3.up * headOffset;

            // Рассчитываем расстояние между призраком и целевой позицией на уровне головы
            float distance = Vector3.Distance(transform.position, targetPosition);

            // Если расстояние больше чем stoppingDistance, призрак продолжает преследовать игрока
            if (distance > stoppingDistance)
            {
                // Направление от призрака к целевой позиции
                Vector3 direction = (targetPosition - transform.position).normalized;

                // Перемещаем призрака к целевой позиции
                transform.position += direction * speed * Time.deltaTime;

                // Поворот призрака в сторону целевой позиции
                transform.LookAt(targetPosition);
            }
            // Если призрак достиг игрока, наносим урон и запускаем анимацию пугания
            else
            {
                DealDamage();
            }
        }
    }

    void DealDamage()
    {
        // Проверяем, может ли игрок получить урон
        if (playerHealth != null && !hasAttacked)
        {
            hasAttacked = true;  // Устанавливаем флаг, что атака произошла
            playerHealth.TakeDamage(damage);
            Debug.Log("Призрак нанес урон: " + damage);

            // Запускаем анимацию пугания
            animator.SetTrigger("Scare");
        }
    }

    // Этот метод вызывается как событие анимации после завершения анимации "Scare"
    public void OnScareAnimationEnd()
    {
        // Запускаем анимацию исчезновения
        animator.SetTrigger("Disappear");
    }

    // Этот метод вызывается как событие анимации после завершения анимации "Disappear"
    public void OnDisappearAnimationEnd()
    {
        // Уничтожаем объект после завершения анимации исчезновения
        Destroy(gameObject);
    }
}
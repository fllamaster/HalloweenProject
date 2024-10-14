using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;  // Максимальное здоровье игрока
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;  // Задаем полное здоровье при старте
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;  // Уменьшаем здоровье на количество урона
        Debug.Log("Игрок получил урон: " + amount + ". Текущее здоровье: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Действия при смерти игрока
        Debug.Log("Игрок погиб!");
        // Здесь можно вызвать конец игры, респаун или другую логику
    }
}
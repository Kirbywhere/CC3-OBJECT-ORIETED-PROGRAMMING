// KIRBY ASAN cc3-1A (under progress)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RPGGame
{
    class Cell
    {
        public char Symbol { get; }
        public bool Passable { get; }

        public Cell(char symbol, bool passable = true)
        {
            Symbol = symbol;
            Passable = passable;
        }
    }

    class Grid
    {
        public int width;
        public int height;
        public Cell[,] cells; // Change access modifier to public

        public Grid(int width, int height)
        {
            this.width = width;
            this.height = height;
            cells = new Cell[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    cells[y, x] = new Cell('#');
                }
            }
        }

        public void GenerateMaze()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        cells[y, x] = new Cell('#');
                    }
                    else
                    {
                        cells[y, x] = new Cell(' ');
                    }
                }
            }

            Stack<(int, int)> stack = new Stack<(int, int)>();
            stack.Push((2, 2));

            while (stack.Count > 0)
            {
                (int currentX, int currentY) = stack.Peek();

                // Get neighbors
                var neighbors = new[] { (currentX + 2, currentY), (currentX - 2, currentY), (currentX, currentY + 2), (currentX, currentY - 2) }
                    .Where(coord => coord.Item1 > 0 && coord.Item1 < width - 1 && coord.Item2 > 0 && coord.Item2 < height - 1)
                    .ToList();

                var unvisitedNeighbors = neighbors.Where(coord => cells[coord.Item2, coord.Item1].Symbol == ' ').ToList();

                if (unvisitedNeighbors.Any())
                {
                    (int nextX, int nextY) = unvisitedNeighbors[new Random().Next(unvisitedNeighbors.Count)];
                    int wallX = (currentX + nextX) / 2;
                    int wallY = (currentY + nextY) / 2;

                    cells[wallY, wallX] = new Cell('#');
                    cells[nextY, nextX] = new Cell('#');
                    stack.Push((nextX, nextY));
                }
                else
                {
                    stack.Pop();
                }
            }
        }

        public (int, int) SpawnEntity(char entitySymbol, int x, int y)
        {
            cells[y, x] = new Cell(entitySymbol);
            return (x, y);
        }

        public void RemoveEntity(int x, int y)
        {
            cells[y, x] = new Cell(' ');
        }

        public void MoveEntity(int oldX, int oldY, int newX, int newY)
        {
            if (cells[newY, newX].Passable)
            {
                (cells[newY, newX], cells[oldY, oldX]) = (cells[oldY, oldX], new Cell(' '));
            }
        }

        public void PrintGrid(int playerX, int playerY)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    char symbol = cells[y, x].Symbol;
                    if (x == playerX && y == playerY)
                    {
                        Console.Write('P');
                    }
                    else
                    {
                        Console.Write(symbol);
                    }
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
        }
    }

    class Entity
    {
        public string Name { get; }
        public string Job { get; }
        public int Health { get; set; }
        public int Damage { get; private set; }
        public int Speed { get; }
        public int Exp { get; private set; }
        private int expThreshold = 100;

        public Entity(string name, string job, int health, int damage, int speed)
        {
            Name = name;
            Job = job;
            Health = health;
            Damage = damage;
            Speed = speed;
            Exp = 0;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0)
            {
                Health = 0;
            }
        }

        public void GainExp(int exp)
        {
            Exp += exp;
            if (Exp >= expThreshold)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            Console.WriteLine($"{Name} leveled up!");
            Health += 2;
            Damage += 2;
            Exp -= expThreshold;
            expThreshold += 50;
        }

        public bool IsAlive()
        {
            return Health > 0;
        }

        public void Attack(Enemy enemy)
        {
            Random random = new Random();
            int attackChoice = random.Next(1, 5);
            int damageDealt = Damage;

            switch (Job)
            {
                case "Archer":
                    if (attackChoice == 2) // Special Attack: Double Shot
                    {
                        damageDealt *= 2;
                        Console.WriteLine($"{Name} used Double Shot and dealt {damageDealt} damage to {enemy.Name}!");
                    }
                    else
                    {
                        // Normal Attack
                    }
                    break;
                case "Warrior":
                    if (attackChoice == 3) // Special Attack: Whirlwind
                    {
                        int whirlwindDamage = Damage;
                        // Implement whirlwind attack
                    }
                    else
                    {
                        // Normal Attack
                    }
                    break;
                case "Mage":
                    if (attackChoice == 4) // Special Attack: Fireball
                    {
                        int fireballDamage = Damage * 3;
                        enemy.TakeDamage(fireballDamage);
                        Console.WriteLine($"{Name} used Fireball and dealt {fireballDamage} damage to {enemy.Name}!");
                    }
                    else
                    {
                        // Normal Attack
                    }
                    break;
                case "Assassin":
                    if (attackChoice == 5) // Special Attack: Stealth Strike
                    {
                        int stealthDamage = Damage * 4;
                        enemy.TakeDamage(stealthDamage);
                        Console.WriteLine($"{Name} used Stealth Strike and dealt {stealthDamage} damage to {enemy.Name}!");
                    }
                    else
                    {
                        // Normal Attack
                    }
                    break;
                default:
                    break;
            }
        }
    }

class Enemy
{
    public string Name { get; }
    public int Health { get; private set; }
    public int Damage { get; }
    public int Speed { get; }

    public Enemy(string name, int health, int damage, int speed)
    {
        Name = name;
        Health = health;
        Damage = damage;
        Speed = speed;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Health = 0;
        }
    }

    public void Attack(Entity entity)
    {
        Random random = new Random();
        int attackChoice = random.Next(1, 5);
        int damageDealt = Damage;

        switch (attackChoice)
        {
            case 1:
                Console.WriteLine($"{Name} attacks with a quick strike!");
                damageDealt = Damage;
                break;
            case 2:
                Console.WriteLine($"{Name} charges up and delivers a powerful blow!");
                damageDealt = Damage * 2;
                break;
            case 3:
                Console.WriteLine($"{Name} performs a rapid flurry of strikes!");
                damageDealt = Damage / 2;
                break;
            case 4:
                Console.WriteLine($"{Name} unleashes a devastating special attack!");
                damageDealt = Damage * 3;
                break;
            default:
                // Implement default action
                Console.WriteLine($"{Name} hesitates and does nothing.");
                break;
        }

        // Apply damage to the target entity
        entity.TakeDamage(damageDealt);
        Console.WriteLine($"Dealt {damageDealt} damage to {entity.Name}.");
    }

    public bool IsAlive()
    {
        return Health > 0;
    }
}

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Dungeon!");

            while (true)
            {
                Console.WriteLine("Choose your job:");
                Console.WriteLine("1. Archer");
                Console.WriteLine("2. Warrior");
                Console.WriteLine("3. Mage");
                Console.WriteLine("4. Assassin");
                Console.Write("Enter the number of your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                string job = "";
                int health = 0, damage = 0, speed = 0;

                switch (choice)
                {
                    case 1:
                        job = "Archer";
                        health = 80;
                        damage = 20;
                        speed = 5;
                        break;
                    case 2:
                        job = "Warrior";
                        health = 100;
                        damage = 30;
                        speed = 3;
                        break;
                    case 3:
                        job = "Mage";
                        health = 60;
                        damage = 40;
                        speed = 2;
                        break;
                    case 4:
                        job = "Assassin";
                        health = 50;
                        damage = 50;
                        speed = 7;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number between 1 and 4.");
                        continue;
                }

                Entity player = new Entity("Player", job, health, damage, speed);
                Console.WriteLine($"You are now a {job}!");

                // Start the game
                PlayGame(player);

                Console.WriteLine("Do you want to play again? (yes/no)");
                string playAgain = Console.ReadLine().ToLower();
                if (playAgain != "yes")
                {
                    break;
                }
            }

            Console.WriteLine("Thank you for playing Blasphemous!");
        }

        static void PlayGame(Entity player)
        {
            const int width = 20;
            const int height = 10;
            const int numEnemies = 5;

            int enemiesDefeated = 0;

            Grid grid = new Grid(width, height);
            grid.GenerateMaze();
            int playerX = 1;
            int playerY = 1;

            List<Enemy> enemies = new List<Enemy>();
            List<(int, int)> enemyPositions = new List<(int, int)>();

            string[] enemyNames = { "Goblin", "Skeleton", "Orc", "Dragon", "Troll", "Witch", "Zombie", "Ghost" };

            for (int i = 0; i < numEnemies; i++)
            {
                string enemyName = i == numEnemies - 1 ? "Boss" : $"{enemyNames[i % enemyNames.Length]} {i + 1}";
                int enemyX, enemyY;
                while (true)
                {
                    enemyX = new Random().Next(1, width - 1);
                    enemyY = new Random().Next(1, height - 1);
            
                    if (grid.cells[enemyY, enemyX].Symbol != 'E')
                    {
                        break;
                    }
                }
                enemies.Add(new Enemy(enemyName, new Random().Next(20, 50), new Random().Next(5, 15), new Random().Next(2, 8)));
                enemyPositions.Add((enemyX, enemyY));
                grid.SpawnEntity('E', enemyX, enemyY);
            }


            while (enemiesDefeated < numEnemies)
            {
                grid.PrintGrid(playerX, playerY);
                Console.WriteLine("\nPlayer Stats:");
                Console.WriteLine($"Name: {player.Name}");
                Console.WriteLine($"Job: {player.Job}");
                Console.WriteLine($"HP: {player.Health}");
                Console.WriteLine($"Damage: {player.Damage}");
                Console.WriteLine($"Speed: {player.Speed}");
                Console.WriteLine($"Exp: {player.Exp}");

                if (!player.IsAlive())
                {
                    Console.WriteLine("You have been defeated!");
                    break;
                }

                Console.WriteLine("\nEnter 'w' to move up, 'a' to move left, 's' to move down, 'd' to move right, or 'q' to quit: ");
                char action = Console.ReadKey().KeyChar;
                Console.WriteLine();

                int newPlayerX = playerX;
                int newPlayerY = playerY;

                switch (action)
                {
                    case 'w':
                        if (playerY > 1 && grid.cells[playerY - 1, playerX].Symbol == ' ')
                        {
                            newPlayerY--;
                        }
                        break;
                    case 'a':
                        if (playerX > 1 && grid.cells[playerY, playerX - 1].Symbol == ' ')
                        {
                            newPlayerX--;
                        }
                        break;
                    case 's':
                        if (playerY < height - 2 && grid.cells[playerY + 1, playerX].Symbol == ' ')
                        {
                            newPlayerY++;
                        }
                        break;
                    case 'd':
                        if (playerX < width - 2 && grid.cells[playerY, playerX + 1].Symbol == ' ')
                        {
                            newPlayerX++;
                        }
                        break;
                    case 'q':
                        Console.WriteLine("You quit the game.");
                        return;
                }

                if (newPlayerX > 0 && newPlayerX < width - 1 && newPlayerY > 0 && newPlayerY < height - 1)
                {
                    playerX = newPlayerX;
                    playerY = newPlayerY;
                }

                foreach (var enemyInfo in enemies.Zip(enemyPositions, (enemy, position) => (enemy, position)))
                {
                    var enemy = enemyInfo.enemy;
                    var enemyPosition = enemyInfo.position;

                    if ((Math.Abs(playerX - enemyPosition.Item1) == 1 && playerY == enemyPosition.Item2) ||
                        (playerX == enemyPosition.Item1 && Math.Abs(playerY - enemyPosition.Item2) == 1))
                    {
                        Console.WriteLine($"Encountered {enemy.Name}!");
                        Console.WriteLine($"Enemy Stats: {enemy.Name}, HP: {enemy.Health}, Damage: {enemy.Damage}, Speed: {enemy.Speed}");

                       while (enemy.IsAlive() && ((Math.Abs(playerX - enemyPosition.Item1) == 1 && playerY == enemyPosition.Item2) ||
                            (playerX == enemyPosition.Item1 && Math.Abs(playerY - enemyPosition.Item2) == 1)))
                        {
                            Console.WriteLine("\nEnter 'a' to attack or 'q' to flee: ");
                            char battleAction = Console.ReadKey().KeyChar;
                            Console.WriteLine();

                            switch (battleAction)
                            {
                                case 'a':
                                    player.Attack(enemy);
                                    if (!enemy.IsAlive())
                                    {
                                        grid.RemoveEntity(enemyPosition.Item1, enemyPosition.Item2);
                                        enemiesDefeated++;
                                        player.GainExp(20); // Assuming fixed EXP gain for each enemy defeated
                                        Console.WriteLine($"{enemy.Name} has been defeated!");
                                    }
                                    else
                                    {
                                        enemy.Attack(player);
                                        if (!player.IsAlive())
                                        {
                                            Console.WriteLine("You have been defeated!");
                                            return;
                                        }
                                    }
                                    break;
                                case 'q':
                                    Console.WriteLine("You fled from battle.");
                                    return;
                            }
                        }
                    }
                }

                Thread.Sleep(100);
                Console.Clear();
            }

            Console.WriteLine("Congratulations! You have defeated all enemies!");
        }
    }
}

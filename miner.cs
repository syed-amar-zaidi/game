using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ConsoleApplication1
{
    class Program
    {
        static const int row = 45;
        static const int col = 130;
        static int maze_row = 33;
        static char[, ] maze = new char[row, col];
        static int maze_col = 105;
        static long fireGenerateCount = 19; // A fire will generate after every iteration multiple of 19
        static int health = 100;
        static int score = 0;
        static int keysLevel1 = 2; // There are two keys in level1
        static int keysLevel2 = 5;
        static string LevelStatus = "LEVEL1";      // This Variable is used to store the current level of manic            // This function is used to lay down manic
        static string isManicInBox = "NOT IN BOX"; // This Variable is used to store the status wheather manic is in the boxstat
        static string isEnemyStuck = "NOT STUCK";
        static int laddder_X = 14; // The Left HEad of Manic should be Equal to this to climb
        static int ladder_Y = 6;
        // Checks Varaible on Player Movement
        static string MoveRightStatus = "Obstacle_Present"; // Variable to keep status for moving in right Wheather there is a obstacle present at right
        static string MoveLeftStatus = "Obstacle_Present";  // Variable to keep status for moving in Left Wheather there is a obstacle present at Left
        static string MoveDownStatus = "Obstacle_Present";  // Variable to keep status for moving down wheather there is a obstacle present at down
        static string MoveUpStatus = "Obstacle_Present";    // Variable to keep Status for moving Up wheather there is a obstacle present at up
        static int ManicJumpCount = 0;
        static int ManicJumpLimit = 6;               // Manic Can jum 5 rows up
        static int keys_captured = 0;                // This Variable will store the currently captured keys by Manic
        static string ManicShould = "NOT FALL";      // To Choose wheather manic should Move Left or He Should fall
        static string ManicRightJumpStatus = "FALL"; // To Choose wheather manic should Move Right or He Should fall
        // int ManicRightJumpLimit = 0;
        // int ManicLeftJumpLimit = 0;
        static int Manic_Current_Row = 0;                 // To Store Manic Current Row position
        static int Manic_Current_Col = 0;                 // To Store Manic Current Column Position
        static string ManicFallingStatus = "NOT FALLING"; // Variable to Store Status of Manic Wheather Manic is Falling or Not
        static string ManicJumpingStatus = "NOT JUMPING";
        static string isManicFrozen = "NOT FROZEN"; // This variable will be freeze manic and manic can not move right or left when he is freezes
        static string isManicLayed = "NOT LAYED";   // This variable to check whather manic is layeed down to ground or not
        static string plateMoving = "RIGHT";        // By default plate will be moving in righ
        char firePreviousItem = ' ';
        static int option = 0;
        static int lives = 3;
        static int ManicInitialPosition_X = 29;
           static int ManicInitialPosition_Y = 6;
            static string RightKey = "UNLOCKED";      // Right and left keys can be locaked or unlocked according to hurdles at front positions
            static string LeftKey = "UNLOCKED";
            static string LadderPosition = "NOTHING"; // This Variable is used to store status of ladder position as RIGHT OR LEFT OF MANIC
            static string isManicClimbing = "NOT CLIMBING";
            static string step1 = "STOP"; // first go near ladder
            static string step2 = "STOP"; // in Secind Step manic will start climbing up
            static string step3 = "STOP"; // step3 will be move right when at the upperend of the ladder
            static int ladder_count = 0;
        // Checks Variable Ends
        static void Main(string[] args)
        {
           
            int face_count = 0; // THis Variable is used to count the arrow keys for when to move manic face
            string left_pressed = "NOT PRESSED";
            string right_pressed = "NOT PRESSED";
            string ClimbingStatus = "END"; // This variable is used to store the status of climbing wheather the manic has started climbing or not
            bool gameRunning = true;
            option = 4;
            while (option != 1 && option != 2)
            {
                Menu();
                if (option == 3)
                {
                    printInstrunctions();
                }
            }
            loadMaze();
            Console.Clear();
            printMaze();
            bool Temp_Manic_Falling_Status;
            int temp_x = 0; // Temperary VAriables to perform different functions on manic location
            int temp_y = 0;
            string temp_direction_status = "RIGHT"; // This Variable is used to store manic face location
            for (int total_turns = lives; total_turns != 0; total_turns--)
            {
                while (gameRunning)
                {
                    if (LevelStatus == "LEVEL2")
                    {
                        if (fireGenerateCount % 19 == 0)
                        {
                            generatefire();
                        }
                        fireGenerateCount++;
                    }
                    MoveFire();
                    movePlate();
                    string temp_direction_status = "RIGHT";
                    Sleep(50);
                    MoveEnemy1Horizontal();
                    Temp_Manic_Falling_Status = isManicFalling();
                    if (!isClimbUpPossible() && LeftKey == "UNLOCKED")
                    { // The left key will be only pressed when there will be no ladder in front of manic to climb
                        // cin >>Temp_Manic_Falling_Status;
                        if (Keyboard.IsKeyPressed(Key.LeftArrow))
                        {
                            if (isManicClimbing == "CLIMBING")
                            {
                                ladder_count++;
                            }
                            if (isManicLayed == "NOT LAYED")
                            {
                                gameRunning = ManicMoveLeft();
                            }
                            if (isManicLayed == "LAYED")
                            {
                                if (temp_direction_status == "RIGHT")
                                {
                                    MoveManicCrawlFace();
                                    temp_direction_status = "LEFT";
                                }
                                if (isManicStandUpPossible())
                                {
                                    isManicFrozen = "NOT FROZEN";
                                    isManicLayed = "NOT LAYED";
                                    ManicStandUp();
                                }
                                if (!isManicStandUpPossible())
                                {
                                    CrawlMoveLeft();
                                }
                                if (isMoveBoxPossible() == "LEFT")
                                {
                                    PlaceManicInBox();
                                }
                            }
                        }
                    }
                    // cin >> Temp_Manic_Falling_Status;
                    if (Keyboard.IsKeyPressed(Key.RightArrow))
                    {
                        if (!isClimbUpPossible() || RightKey == "UNLOCKED") // Fix Required at  ||
                        {
                            if (isManicLayed == "NOT LAYED")
                            {
                                gameRunning = ManicMoveRight();
                            }
                            if (isManicLayed == "LAYED")
                            {
                                if (temp_direction_status == "LEFT")
                                {
                                    MoveManicCrawlFace();
                                    temp_direction_status = "RIGHT";
                                }
                                CrawlMoveRight();
                                if (isMoveBoxPossible() == "RIGHT")
                                {
                                    PlaceManicInBox();
                                    if (isManicInBox == "IN BOX")
                                    {
                                        if (LevelStatus == "LEVEL1")
                                        {
                                            if (keys_captured == keysLevel1)
                                            {
                                                LevelStatus = "LEVEL2";
                                                ChangeMaze();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (ManicFallingStatus == "FALLING" && ManicJumpingStatus == "NOT JUMPING" && isManicClimbing == "NOT CLIMBING")
                    {
                        FellTheManic();
                    }
                    if (ManicJumpingStatus == "NOT JUMPING" && ManicFallingStatus == "NOT FALLING")
                    {
                        if (Keyboard.IsKeyPressed(Key.Space))
                        {
                            if (isManicFrozen == "NOT FROZEN")
                            { // While Manic is Frozen User cannot jump
                                gameRunning = JumpManic();
                                ManicJumpCount++;
                                ManicJumpingStatus = "JUMPING";
                            }
                        }
                    }
                    if (ManicJumpingStatus == "JUMPING")
                    {
                        if (ManicJumpCount == ManicJumpLimit)
                        {
                            ManicJumpingStatus = "NOT JUMPING";
                            ManicJumpCount = 0;
                        }
                        else
                        {
                            gameRunning = JumpManic();
                            ManicJumpCount++;
                        }
                    }
                    if (Keyboard.IsKeyPressed(Key.NUMPAD0)))
                        {
                            if (isClimbDownPossible() && ladder_count != 0)
                            {
                                ladder_count = 0;
                            }
                            if ((isClimbUpPossible() || isClimbDownPossible()) && isManicClimbing == "NOT CLIMBING")
                            {
                                LadderPosition = isClimbPosition();

                                SetManicCurrentLocation();
                                temp_x = Manic_Current_Row;
                                temp_y = Manic_Current_Col;
                            }
                            ManicClimb( temp_x, temp_y);
                        }

                    if (Keyboard.IsKeyPressed(Key.NUMPAD5))
                    {
                        // This Function will check wheather manic can crawl according to his current location
                        //  And it will assign isManicLayed Varible to "LAYED" if He will be possible to lay down
                        ManicLayDown();
                        if (isManicInBox == "IN BOX")
                        {
                            PlaceManicOutBox();
                        }
                    }

                    if (Keyboard.IsKeyPressed(Key.ESCAPE))
                    {
                Console.Write("Game Over";
                gameRunning = false;
                    }
                    PrintLives();
                    PrintKeys();
                    CalculateScore();
                    PrintScore();
                    PrintHealth();
                    // save(lives);
                    if (health == 0)
                    {
                        gameRunning = false;
                    }
                }
                if (LevelStatus == "LEVEL2")
                {
                    if (keys_captured == keysLevel2)
                    {
                        GameOver();
                        total_turns = 0;
                        break;
                    }
                }
                health = 100;
                CalculateLives();
                PrintLives(); // When live will be minus it have to remove last location
                ResetLevel1(ManicInitialPosition_X, ManicInitialPosition_Y);
                gameRunning = true;
            }
            return 0;
        }
        static void Menu()
        {
            Console.Clear();
            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine();
            }
            Console.WriteLine("\t\t\tMenu>>");
            Console.WriteLine("\t\t\t1_Start New Game");
            Console.WriteLine("\t\t\t2_Load Game");
            Console.WriteLine("\t\t\t3_See Instructions");
            Console.WriteLine("\t\t\t4_Exit");
            Console.WriteLine("\t\t\tEnter your Option");
            option = int.Parse(Console.ReadLine());
        }
        static void save()
        {
            string path = "C:\\Users\\kali\\Videos\\manic\\Load.txt" StreamWriter file = new StreamWriter(path);
            string line;
            if (File.Exists(path))
            {
                for (int a = 0; a < maze_row; a++)
                {
                    for (int b = 0; b < maze_col; b++)
                    {
                        file.Write(maze[a, b]);
                    }
                    File.WriteLine();
                }
            }

            file.Flush();
            file.Close();
            saveImpVariables();
        }
        static void saveImpVariables()
        {
            path = "C:\\Users\\kali\\Videos\\manic\\impVariables.txt";
            StreamWriter file = new StreamWriter(path);
            string line;
            if (File.Exists(path))
            {
                Console.WriteLine(isManicFrozen);
                Console.WriteLine(ManicFallingStatus);
                Console.WriteLine(ManicJumpingStatus);
                Console.WriteLine(isManicInBox);
                Console.WriteLine(isManicLayed);
                Console.WriteLine(score);
                Console.WriteLine(health);
                Console.WriteLine(lives);
                Console.WriteLine(keys_captured);
                Console.Write(LevelStatus);
            }
            file.Flush();
            file.close();
        }
        static void loadImpVariables()
        {
            string path = "C:\\Users\\kali\\Videos\\manic\\imVariables.txt";
            StreamReader file = new StreamReader(path);
            string line;
            if (File.Exists(path))
            {
                isManicFrozen = line;
                ManicFallingStatus = file.ReadLine();
                ManicJumpingStatus = file.ReadLine();
                isManicInBox = file.ReadLine();
                isManicLayed = file.ReadLine();
                score = int.Parse(file.ReadLine());
                health = int.Parse(file.ReadLine());
                lives = int.Parse(file.ReadLine());
                keys_captured = int.Parse(ReadLine())
                LevelStatus = file.ReadLine();
            }
        }

        file.close();
    }
    static void CalculateLives()
    {
        lives = lives - 1;
    }
    static void PrintLives()
    {
     Console.SetCursorPosition(12,42);
    Console.Write("LIVES::\t{0}", lives);
    }
    static void CalculateKeys(int &key_captured)
    {
        key_captured += 1;
    }
    static void PrintKeys()
    {
        int temp_count = 0;
        for (int x = 0; x < maze_row; x++)
        { // Identifying how many keys are left in maze
            for (int y = 0; y < maze_col; y++)
            {
                if (maze[x,y] == '!')
                {
                    temp_count++;
                }
            }
        }
        if (LevelStatus == "LEVEL1")
        {
            keys_captured = keysLevel1 - temp_count;
        }
        if (LevelStatus == "LEVEL2")
        {
            keys_captured = keysLevel2 - temp_count;
        }
    console.SetCursorPosition(12,44);
    Console.Write("Keys :\t");
    for (int temp_idx = keys_captured; temp_idx != 0; temp_idx--)
    {
        Console.Write(" ! ");
    }
    }
   static void loadMaze()
    {
        if (option == 2)
        {
            loadImpVariables();
        }
        string line;
        int row_idx = 0;
        fstream file;
        if (option == 2)
        {
            option = 0;
            string path = "C:\\Users\\kali\\Videos\\Load.txt";
        }
        else if (option == 1)
        {
            option = 0;
            string path = "C:\\Users\\kali\\Videos\\maze.txt"
        }
        else if (LevelStatus == "LEVEL1")
        {
            string path = "C:\\Users\\kali\\Videos\\maze.txt";
        }
        else if (LevelStatus == "LEVEL2")
        {
            path = "C:\\Users\\kali\\Videos\\level2.txt";
        }
        StreamReader file = new StreamReader(path);
        while ((line = file.ReadLine()) != null)
        {
            
            for (int col_idx = 0; line[col_idx] != '\0'; col_idx++)
            {
                maze[row_idx,col_idx] = line[col_idx];
            }
            row_idx++;
        }
        file.close();
    }
    static void printMaze()
    {
        for (int row_idx = 0; row_idx < maze_row; row_idx++)
        {
            for (int col_idx = 0; col_idx < maze_col; col_idx++)
            {
            Console.Write(maze[row_idx,col_idx]);
            }
        Console.WriteLine();
        }
    }
    static void ResetLevel1()
    {
        RemoveManic();
        maze[position_X,ManicInitialPosition_Y] = '/';
        maze[position_X,ManicInitialPosition_Y + 1] = '\\';
        maze[ManicInitialPosition_X+ 1,ManicInitialPosition_Y] = '\\';
        maze[ManicInitialPosition_X+ 1,ManicInitialPosition_Y + 1] = '/';
        maze[ManicInitialPosition_X+ 2,ManicInitialPosition_Y] = '/';
        maze[ManicInitialPosition_X+ 2,ManicInitialPosition_Y + 1] = '\\';
        Console.SetCursorPosition(ManicInitialPosition_Y, ManicInitialPosition_X);
    Console.Write('/');
    Console.SetCursorPosition(ManicInitialPosition_Y + 1, ManicInitialPosition_X);
    Console.Write('\\');
    Console.SetCursorPosition(ManicInitialPosition_Y, ManicInitialPosition_X+ 1);
    Console.Write('\\');
    Console.SetCursorPosition(ManicInitialPosition_Y + 1, ManicInitialPosition_X+ 1);
    Console.Write('/');
    Console.SetCursorPosition(ManicInitialPosition_Y, ManicInitialPosition_X+ 2);
    Console.Write('/');
    Console.SetCursorPosition(ManicInitialPosition_Y + 1, ManicInitialPosition_X+ 2);
    Console.Write('\\');
    }
    static bool isRightMovePossible(int temp_row_idx, int temp_col_idx)
    {
        int temp_count = 0;
        if (maze[temp_row_idx,temp_col_idx + 1] == ' ' || maze[temp_row_idx,temp_col_idx + 1] == '0' || maze[temp_row_idx,temp_col_idx + 1] == '+' || maze[temp_row_idx,temp_col_idx + 1] == '!')
        {
            temp_count++;
        }
        if (maze[temp_row_idx + 1,temp_col_idx + 1] == ' ' || maze[temp_row_idx + 1,temp_col_idx + 1] == '0' || maze[temp_row_idx + 1,temp_col_idx + 1] == '+' || maze[temp_row_idx + 1,temp_col_idx + 1] == '!')
        {
            temp_count++;
        }
        if (maze[temp_row_idx + 2,temp_col_idx + 1] == ' ' || maze[temp_row_idx + 2,temp_col_idx + 1] == '0' || maze[temp_row_idx + 2,temp_col_idx + 1] == '+' || maze[temp_row_idx + 2,temp_col_idx + 1] == '!')
        {
            temp_count++;
        }
        if (temp_count == 3)
        {
            if (isManicFrozen == "NOT FROZEN")
            { // If manic is not on the ladder then hw is not freezed and he can move right
                return true;
            }
        }
        return false;
    }
    static bool isLeftMovePossible(int temp_row_idx, int temp_col_idx)
    {
        int temp_count = 0;
        if (maze[temp_row_idx,temp_col_idx - 1] == ' ' || maze[temp_row_idx,temp_col_idx - 1] == '0' || maze[temp_row_idx,temp_col_idx - 1] == '+' || maze[temp_row_idx,temp_col_idx - 1] == '|' || maze[temp_row_idx,temp_col_idx - 1] == '_' || maze[temp_row_idx,temp_col_idx - 1] == '!')
        {
            temp_count++;
        }
        if (maze[temp_row_idx + 1,temp_col_idx - 1] == ' ' || maze[temp_row_idx + 1,temp_col_idx - 1] == '0' || maze[temp_row_idx + 1,temp_col_idx - 1] == '+' || maze[temp_row_idx + 1,temp_col_idx - 1] == '|' || maze[temp_row_idx + 1,temp_col_idx - 1] == '_' || maze[temp_row_idx + 1,temp_col_idx - 1] == '!')
        {
            temp_count++;
        }
        if (maze[temp_row_idx + 2,temp_col_idx - 1] == ' ' || maze[temp_row_idx + 2,temp_col_idx - 1] == '0' || maze[temp_row_idx + 2,temp_col_idx - 1] == '+' || maze[temp_row_idx + 2,temp_col_idx - 1] == '|' || maze[temp_row_idx + 2,temp_col_idx - 1] == '_' || maze[temp_row_idx + 2,temp_col_idx - 1] == '!')
        {
            temp_count++;
        }
        if (temp_count == 3)
        {
            if (isManicFrozen == "NOT FROZEN")
            { // If manic is not on ladder then he can move at left and he is in NOT FROZEN P
                return true;
            }
        }
        return false;
    }
    static bool isDownMovePossible(int temp_row_idx, int temp_col_idx)
    {
        if (maze[temp_row_idx + 1,temp_col_idx] == ' ' || maze[temp_row_idx + 1,temp_col_idx] == '0' || maze[temp_row_idx + 1,temp_col_idx + 1] == '+' || maze[temp_row_idx + 1,temp_col_idx + 1] == '!' || maze[temp_row_idx + 1,temp_col_idx] == '!' || maze[temp_row_idx + 1,temp_col_idx + 1] == '_') // This If condition require editing in changing in condition when it will meet up with enemy
        {
            return true;
        }
        return false;
    }
    static bool isUpHurdlePresent(int temp_row_idx, int temp_col_idx)
    {
        if ((((maze[temp_row_idx - 1,temp_col_idx] == ' ' && maze[temp_row_idx - 1,temp_col_idx + 1] == ' ') || (maze[temp_row_idx - 1,temp_col_idx] == '_' && maze[temp_row_idx - 1,temp_col_idx + 1] == '_') || (maze[temp_row_idx - 1,temp_col_idx] == '!' || maze[temp_row_idx - 1,temp_col_idx + 1] == '!'))) && (maze[temp_row_idx - 1,temp_col_idx] != '$' && maze[temp_row_idx - 1,temp_col_idx + 1] != '$'))
        {
            return false;
        }
        return true;
    }
    static bool ManicMoveLeft()
    {
        bool temper = true; // Used to check isLeftMovePossibleCondition Only for one time
        SetManicCurrentLocation();
        for (int row_idx = Manic_Current_Row; row_idx < Manic_Current_Row + 3; row_idx++)
        {
            for (int col_idx = Manic_Current_Col; col_idx < Manic_Current_Col + 2; col_idx++)
            {
                if (temper)
                {
                    if (isLeftMovePossible(row_idx, col_idx))
                    {
                        if (maze[row_idx + 1,col_idx - 1] == '0' || maze[row_idx + 1,col_idx - 1] == '+' || maze[row_idx + 2,col_idx - 1] == '0' || maze[row_idx + 2,col_idx - 1] == '+' || maze[row_idx,col_idx - 1] == '0' || maze[row_idx,col_idx - 1] == '+')
                        {
                            return false;
                        }
                        if (maze[row_idx + 1,col_idx - 1] == '!' || maze[row_idx + 1,col_idx - 1] == '!' || maze[row_idx + 2,col_idx - 1] == '!' || maze[row_idx + 2,col_idx - 1] == '!' || maze[row_idx,col_idx - 1] == '!' || maze[row_idx,col_idx - 1] == '!')
                        {
                            keys_captured += 1; // This if condition is checking if there is a key in left then it will update the keys_captured variable
                        }
                        MoveLeftStatus = "No_Obstacle";
                    }
                    temper = false;
                }
                if (MoveLeftStatus == "No_Obstacle")
                {
                    if (maze[row_idx,col_idx] == '/')
                    {
                        maze[row_idx,col_idx] = ' ';
                        Console.SetCursorPosition(col_idx, row_idx);
                    Console.Write(' ');
                    maze[row_idx,col_idx - 1] = '/';
                    Console.SetCursorPosition(col_idx - 1, row_idx);
                    Console.Write('/');
                    }
                    else if (maze[row_idx,col_idx] == '\\')
                    {
                        maze[row_idx,col_idx] = ' ';
                        Console.SetCursorPosition(col_idx, row_idx);
                    Console.Write(' ');
                    maze[row_idx,col_idx - 1] = '\\';
                    Console.SetCursorPosition(col_idx - 1, row_idx);
                    Console.Write('\\');
                    }
                }
            }
        }
        MoveLeftStatus = "Obstacle";
        return true;
    }
    static bool ManicMoveRight()
    {
        bool temper = true;
        SetManicCurrentLocation();
        for (int row_idx = Manic_Current_Row; row_idx < Manic_Current_Row + 3; row_idx++)
        {
            for (int col_idx = Manic_Current_Col + 1; col_idx > Manic_Current_Col - 1; col_idx--)
            {
                if(temper)
                {
                    if (isRightMovePossible(row_idx, col_idx))
                    {
                        if (maze[row_idx + 1,col_idx + 1] == '0' || maze[row_idx + 1,col_idx + 1] == '+' || maze[row_idx + 2,col_idx + 1] == '0' || maze[row_idx + 2,col_idx + 1] == '+' || maze[row_idx,col_idx + 1] == '0' || maze[row_idx,col_idx + 1] == '+')
                        {
                            return false;
                        }
                        if (maze[row_idx + 1,col_idx + 1] == '!' || maze[row_idx + 1,col_idx + 1] == '!' || maze[row_idx + 2,col_idx + 1] == '!' || maze[row_idx + 2,col_idx + 1] == '!' || maze[row_idx,col_idx + 1] == '!' || maze[row_idx,col_idx + 1] == '!')
                        {
                            keys_captured += 1;
                        }
                        MoveRightStatus = "No_Obstacle";
                    }
                    temper = false;
                }
                if (MoveRightStatus == "No_Obstacle")
                {
                    if (maze[row_idx,col_idx] == '/')
                    {
                        maze[row_idx,col_idx] = ' ';
                        Console.SetCursorPosition(col_idx, row_idx);
                    Console.Write(' ');
                    maze[row_idx,col_idx + 1] = '/';
                    Console.SetCursorPosition(col_idx + 1, row_idx);
                    Console.Write(maze[row_idx,col_idx + 1]);
                    }
                    else if (maze[row_idx,col_idx] == '\\')
                    {
                        maze[row_idx,col_idx] = ' ';
                        Console.SetCursorPosition(col_idx, row_idx);
                    Console.Write(' ');
                    maze[row_idx,col_idx + 1] = '\\';
                    Console.SetCursorPosition(col_idx + 1, row_idx);
                    Console.Write(maze[row_idx,col_idx + 1]);
                    }
                }
            }
        }
        MoveRightStatus = "Obstacle_Present";
        return true;
    }
    static bool ManicMoveDown()
    {
        SetManicCurrentLocation();
        for (int temp_row = Manic_Current_Row + 2; temp_row >= Manic_Current_Row; temp_row--)
        {
            for (int temp_col = Manic_Current_Col; temp_col < Manic_Current_Col + 2; temp_col++)
            {
                if (isDownMovePossible(temp_row, temp_col))
                {
                    if (maze[temp_row + 3,temp_col] == '0' || maze[temp_row + 3,temp_col] == '+' || maze[temp_row + 3,temp_col + 1] == '0' || maze[temp_row + 3,temp_col + 1] == '+')
                    {
                        return false;
                    }
                    if (maze[temp_row + 3,temp_col] == '!' || maze[temp_row + 3,temp_col] == '!' || maze[temp_row + 3,temp_col + 1] == '!' || maze[temp_row + 3,temp_col + 1] == '!')
                    {
                        keys_captured += 1;
                    }
                    MoveDownStatus = "No_Obstacle"; // Area is cleared so further processing is possible
                }
                if (MoveDownStatus == "No_Obstacle")
                {
                    if (maze[temp_row,temp_col] == '/')
                    {
                        maze[temp_row,temp_col] = ' ';
                        Console.SetCursorPosition(temp_col, temp_row);
                    Console.Write(' ');
                    maze[temp_row + 1,temp_col] = '/';
                    Console.SetCursorPosition(temp_col, temp_row + 1);
                    Console.Write(maze[temp_row + 1,temp_col]);
                    }
                    else if (maze[temp_row,temp_col] == '\\')
                    {
                        maze[temp_row,temp_col] = ' ';
                        Console.SetCursorPosition(temp_col, temp_row);
                    Console.Write(' ');
                    maze[temp_row + 1,temp_col] = '\\';
                    Console.SetCursorPosition(temp_col, temp_row + 1);
                    Console.Write(maze[temp_row + 1,temp_col]);
                    }
                }
            }
        }
        MoveDownStatus = "Obstacle_Present";
        return true;
    }
    static bool ManicMoveUp()
    {
        bool temp_flag = true;
        SetManicCurrentLocation();
        for (int temp_row = Manic_Current_Row; temp_row < Manic_Current_Row + 3; temp_row++)
        {
            for (int temp_col = Manic_Current_Col; temp_col < Manic_Current_Col + 2; temp_col++)
            {
                if (maze[Manic_Current_Row - 1,Manic_Current_Col] == '!' || maze[Manic_Current_Row - 1,Manic_Current_Col + 1] == '!')
                {
                    keys_captured += 1;
                }
                if (temp_flag)
                {
                    if (!isUpHurdlePresent(temp_row, temp_col))
                    {
                        MoveUpStatus = "No_Obstacle";
                    }
                    else
                    {
                        MoveUpStatus = "Obstacle_Present";
                    }
                    temp_flag = false;
                }
                if (MoveUpStatus == "No_Obstacle")
                {
                    if (maze[temp_row,temp_col] == '/')
                    {
                        maze[temp_row,temp_col] = ' ';
                        Console.SetCursorPosition(temp_col, temp_row);
                    Console.Write(' ');
                    maze[temp_row - 1,temp_col] = '/';
                    Console.SetCursorPosition(temp_col, temp_row - 1);
                    Console.Write('/');
                    }
                    else if (maze[temp_row,temp_col] == '\\')
                    {
                        maze[temp_row,temp_col] = ' ';
                        Console.SetCursorPosition(temp_col, temp_row);
                    Console.Write(' ');
                    maze[temp_row - 1,temp_col] = '\\';
                    Console.SetCursorPosition(temp_col, temp_row - 1);
                    Console.Write('\\');
                    }
                }
            }
        }
        MoveUpStatus = "Obstacle_Present";
        return true;
    }
    static void SetManicCurrentLocation()
    {
        string isLocationFound = "NOT";
        for (int temp_row_idx = 0; temp_row_idx < maze_row; temp_row_idx++)
        {
            for (int temp_col_idx = 0; temp_col_idx < maze_col; temp_col_idx++)
            {
                if (maze[temp_row_idx,temp_col_idx] == '/')
                {
                    Manic_Current_Row = temp_row_idx;
                    Manic_Current_Col = temp_col_idx;
                    isLocationFound = "FOUND";
                    break;
                }
            }
            if (isLocationFound == "FOUND")
            {
                break;
            }
        }
    }
    static bool JumpManic()
    {
        bool temp_flag = true;
        temp_flag = ManicMoveUp();
        return temp_flag;
    }
    static bool isManicFalling()
    {
        SetManicCurrentLocation();
        if (maze[Manic_Current_Row + 3,Manic_Current_Col] == ' ' && maze[Manic_Current_Row + 3,Manic_Current_Col + 1] == ' ' || (maze[Manic_Current_Row + 3,Manic_Current_Col] == '!' || maze[Manic_Current_Row + 3,Manic_Current_Col + 1] == '!'))
        {
            ManicFallingStatus = "FALLING";
            return true;
        }
        ManicFallingStatus = "NOT FALLING";
        return false;
    }
   static bool FellTheManic()
    {
        bool temp_flag = true;
        if (isManicFalling())
        {
            temp_flag = ManicMoveDown();
        }
        return temp_flag;
    }
    static bool isRightEnemy1Possible(int temp_row_idx, int temp_col_idx)
    {
        if (maze[temp_row_idx + 2,temp_col_idx + 2] == ' ')
        {
            return true;
        }
        return false;
    }
    static bool isLeftEnemy1Possible(int temp_row_idx, int temp_col_idx)
    {
        if (maze[temp_row_idx + 3,temp_col_idx - 2] == ' ')
        {
            return false;
        }
        return true;
    }
    static void MoveEnemy1Right()
    {
        string EnemyRightStatus = "Obstacle";
        for (int row_idx = 0; row_idx < maze_row; row_idx++)
        {
            for (int col_idx = 0; col_idx < maze_col; col_idx++)
            {
                if (maze[row_idx,col_idx] == '0')
                {
                    if (isRightEnemy1Possible(row_idx, col_idx))
                    {
                        EnemyRightStatus = "No Obstacle";
                    }
                    else
                    {
                        isEnemyStuck = "STUCK";
                    }
                }
                if (EnemyRightStatus == "No Obstacle")
                {
                    if (maze[row_idx,col_idx] == '0')
                    { // Moving the Head of Enemy
                        maze[row_idx,col_idx] = ' ';
                        Console.SetCursorPosition(col_idx, row_idx);
                    Console.Write(' ');
                    maze[row_idx,col_idx + 1] = '0';
                    Console.SetCursorPosition(col_idx + 1, row_idx);
                    Console.Write('0');
                    break;
                    }
                    if (maze[row_idx,col_idx] == '+')
                    { // Moving the Body of Enemy
                        maze[row_idx,col_idx] = ' ';
                        Console.SetCursorPosition(col_idx, row_idx);
                    Console.Write(' ');
                    maze[row_idx,col_idx + 1] = '+';
                    Console.SetCursorPosition(col_idx + 1, row_idx);
                    Console.Write('+');
                    col_idx = col_idx + 1;
                    }
                }
            }
        }
    }
    static void MoveEnemy1Left()
    {
        string EnemyLeftStatus = "Obstacle";
        for (int row_idx = 0; row_idx < maze_row; row_idx++)
        {
            for (int col_idx = 0; col_idx < maze_col; col_idx++)
            {
                if (maze[row_idx,col_idx] == '0')
                {
                    if (isLeftEnemy1Possible(row_idx, col_idx))
                    {
                        EnemyLeftStatus = "No Obstacle";
                    }
                    else
                    {
                        isEnemyStuck = "NOT STUCK";
                    }
                }
                if (EnemyLeftStatus == "No Obstacle")
                {
                    if (maze[row_idx,col_idx] == '0')
                    { // Moving the Head of Enemy
                        maze[row_idx,col_idx] = ' ';
                        Console.SetCursorPosition(col_idx, row_idx);
                    Console.Write(' ');
                    maze[row_idx,col_idx - 1] = '0';
                    Console.SetCursorPosition(col_idx - 1, row_idx);
                    Console.Write('0');
                    break;
                    }
                    if (maze[row_idx,col_idx] == '+')
                    { // Moving the Body of Enemy
                        maze[row_idx,col_idx] = ' ';
                        Console.SetCursorPosition(col_idx, row_idx);
                    Console.Write(' ');
                    maze[row_idx,col_idx - 1] = '+';
                    Console.SetCursorPosition(col_idx - 1, row_idx);
                    Console.Write('+');
                    col_idx = col_idx - 1;
                    }
                }
            }
        }
    }
   static void MoveEnemy1Horizontal()
    {
        if (isEnemyStuck == "STUCK")
        {
            MoveEnemy1Left();
        }
        else if (isEnemyStuck == "NOT STUCK")
        {
            MoveEnemy1Right();
        }
    }
    static bool isClimbUpPossible()
    {
        RightKey = "UNLOCKED";
        LeftKey = "UNLOCKED";
        SetManicCurrentLocation();
        // This Condition is used to check wheather there is a ladder present at the front of manic to climb
        if (maze[Manic_Current_Row,Manic_Current_Col - 1] == '|' && maze[Manic_Current_Row,Manic_Current_Col - 2] == '_' && maze[Manic_Current_Row,Manic_Current_Col - 3] == '_')
        {
            LeftKey = "LOCKED"; // User cannot move left if left key is locked
            RightKey = "UNLOCKED";
            return true;
        }
        else if (maze[Manic_Current_Row,Manic_Current_Col + 2] == '|' && maze[Manic_Current_Row,Manic_Current_Col + 3] == '_' && maze[Manic_Current_Row,Manic_Current_Col + 4] == '_')
        {
            RightKey = "LOCKED";
            LeftKey = "UNLOCKED";
            return true;
        }
        return false;
    }
   static bool isClimbDownPossible()
    {
        // This function is used to check wheather manic can climb down or not
        SetManicCurrentLocation();
        if ((maze[Manic_Current_Row + 4,Manic_Current_Col - 2] == '_' || maze[Manic_Current_Row + 4,Manic_Current_Col - 1] == '_') && (maze[Manic_Current_Row + 3,Manic_Current_Col] == '#' && maze[Manic_Current_Row + 3,Manic_Current_Col + 1] == '#'))
        {
            return true;
        }
        return false;
    }
    static void RemoveManic()
    { // Removing Manic From the ladder Upper Position

        for (int i = 0; i < maze_row; i++)
        {
            for (int a = 0; a < maze_col; a++)
            {
                if (maze[i,a] == '/' || maze[i,a] == '\\' || maze[i,a] == '-')
                {
                    maze[i,a] = ' ';
                    Console.SetCursorPosition(a, i);
                Console.Write(' ');
                }
            }
        }
    }
    static void UpdateManicLadderPositionUp(int temp_x, int temp_y)
    {                  // This Function is Used to Lift the Manic To Upper Side From Ladder For Level 1
        RemoveManic(); // It will remove manic from the ladder and then manic will be shown outside of the ladder
        maze[temp_x - 7,temp_y] = '/';
        Console.SetCursorPosition(temp_y, temp_x - 7);
    Console.Write('/');
    maze[temp_x - 7,temp_y + 1] = '\\';
    Console.SetCursorPosition(temp_y + 1, temp_x - 7);
    Console.Write('\\');
    maze[temp_x - 6,temp_y] = '\\';
    Console.SetCursorPosition(temp_y, temp_x - 6);
    Console.Write('\\');
    maze[temp_x - 6,temp_y + 1] = '/';
    Console.SetCursorPosition(temp_y + 1, temp_x - 6);
    Console.Write('/');
    maze[temp_x - 5,temp_y] = '/';
    Console.SetCursorPosition(temp_y, temp_x - 5);
    Console.Write('/');
    maze[temp_x - 5,temp_y + 1] = '\\';
    Console.SetCursorPosition(temp_y + 1, temp_x - 5);
    Console.Write('\\');
    }
    static void UpdateManicLadderPositionDown(int temp_x, int temp_y)
    {
        RemoveManic(); // Removing manic from the ladder
        // Starting snippet of code to help manic to move out of the ladder
        maze[temp_x + 7,temp_y] = '/';
        Console.SetCursorPosition(temp_y, temp_x + 7);
    Console.Write('/');
    maze[temp_x + 7,temp_y + 1] = '\\';
    Console.SetCursorPosition(temp_y + 1, temp_x + 7);
    Console.Write('\\');
    maze[temp_x + 8,temp_y] = '\\';
    Console.SetCursorPosition(temp_y, temp_x + 8);
    Console.Write('\\');
    maze[temp_x + 8,temp_y + 1] = '/';
    Console.SetCursorPosition(temp_y + 1, temp_x + 8);
    Console.Write('/');
    maze[temp_x + 9,temp_y] = '/';
    Console.SetCursorPosition(temp_y, temp_x + 9);
    Console.Write('/');
    maze[temp_x + 9,temp_y + 1] = '\\';
    Console.SetCursorPosition(temp_y + 1, temp_x + 9);
    Console.Write('\\');
    // Ending snippet code
    }
    static bool isCrawlPositionPossible() // This Function is used to check wheather manic can come into a crawling position
    {
        int temp_count = 0;
        SetManicCurrentLocation();
        for (int i = 2; i <= 5; i++)
        {
            if (maze[Manic_Current_Row + 2,Manic_Current_Col + i] == ' ' && maze[Manic_Current_Row + 1,Manic_Current_Col + i] == '#') // This if condition is used to check wheather at the next location he can lay down to crawl
            {
                temp_count++;
            }
        }
        if (temp_count == 4)
        {
            return true;
        }
        return false;
    }
    static void ManicLayDown()
    {
        if (isCrawlPositionPossible())
        {
            SetManicCurrentLocation();
            RemoveManic();
            isManicLayed = "LAYED";
        }
        if (isManicLayed == "LAYED")
        {
            int x = Manic_Current_Row + 2; // Choosen location for manic to be lay down
            int y = Manic_Current_Col + 2;
            maze[x,y + 2] = '-';
            maze[x,y + 3] = '-';
            maze[x,y + 4] = '/';
            maze[x,y + 5] = '\\';
            Console.SetCursorPosition(y + 2, x);
        Console.Write('-');
        Console.SetCursorPosition(y + 3, x);
        Console.Write('-');
        Console.SetCursorPosition(y + 4, x);
        Console.Write('/');
        Console.SetCursorPosition(y + 5, x);
        Console.Write('\\');
        }
    }
    static void CrawlMoveRight()
    {
        char temp_char = ' ';
        // bool temp_found_status = false;
        for (int x = 0; x < maze_row; x++)
        {
            for (int y = maze_col - 1; y != 0; y--)
            {
                temp_char = maze[x,y];
                if (temp_char == '\\' || temp_char == '/' || temp_char == '-')
                { // This will match the head of manic when he is at crawling position and will move one step right
                    maze[x,y] = ' ';
                    Console.SetCursorPosition(y, x);
                Console.Write(' ');
                maze[x,y + 1] = temp_char;
                Console.SetCursorPosition(y + 1, x);
                Console.Write(temp_char);
                }
            }
        }
    }
    static string ManicCrawlingFaceLocation() // This Function is used to Find the Manic Face side when he is crawling
    {
        bool isFound = false;
        string temp_face_location = "NOTHING";
        int temp_tail_y = 0; // These Variables are used to compare y index of crawling manic tail and head to guess the current facing location
        int temp_head_y = 0;
        for (int x = 30; x < maze_row; x++)
        { // Searching for Location of MAnic While Crawling
            for (int y = 0; y < maze_col; y++)
            {
                if (maze[x,y] == '-')
                {
                    temp_tail_y = y;
                    isFound = true;
                }
                else if (maze[x,y] == '/' || maze[x,y] == '\\')
                {
                    temp_head_y = y;
                    isFound = true;
                }
                else
                {
                    isFound = false;
                }
            }
            if (isFound)
            {
                break;
            }
        }
        if (temp_head_y > temp_tail_y)
        {
            temp_face_location = "RIGHT";
        }
        else
        {
            temp_face_location = "LEFT";
        }
        return temp_face_location;
    }
    static void MoveManicCrawlFace()
    { // This Function is used to Turn the face of Manic when he is crawling
        bool isFound = false;
        string temp_face_location = ManicCrawlingFaceLocation();
        if (temp_face_location == "LEFT")
        {
            for (int x = 30; x < maze_row; x++)
            { // Searching for head and overwriting it
                for (int y = 0; y < maze_col; y++)
                {
                    if (maze[x,y] == '/')
                    {
                        RemoveManic();
                        maze[x,y] = '-';
                        maze[x,y + 1] = '-';
                        maze[x,y + 2] = '/';
                        maze[x,y + 3] = '\\';
                        Console.SetCursorPosition(y, x);
                    Console.Write('-');
                    Console.SetCursorPosition(y + 1, x);
                    Console.Write('-');
                    Console.SetCursorPosition(y + 2, x);
                    Console.Write('/');
                    Console.SetCursorPosition(y + 3, x);
                    Console.Write('\\');
                    isFound = true;
                    break;
                    }
                }
                if (isFound)
                {
                    break;
                }
            }
        }
        else if (temp_face_location == "RIGHT")
        {
            for (int x = 30; x < maze_row; x++)
            { // Searching for tail and overwriting it
                for (int y = 0; y < maze_col; y++)
                {
                    if (maze[x,y] == '-')
                    {
                        RemoveManic();
                        maze[x,y] = '/';
                        maze[x,y + 1] = '\\';
                        maze[x,y + 2] = '-';
                        maze[x,y + 3] = '-';
                        Console.SetCursorPosition(y, x);
                    Console.Write('/');
                    Console.SetCursorPosition(y + 1, x);
                    Console.Write('\\');
                    Console.SetCursorPosition(y + 2, x);
                    Console.Write('-');
                    Console.SetCursorPosition(y + 3, x);
                    Console.Write('-');
                    isFound = true;
                    break;
                    }
                }
                if(isFound)
                {
                    break;
                }
            }
        }
    }
    static void CrawlMoveLeft()
    {
        char temp_char = ' ';
        for (int x = 0; x < maze_row; x++)
        {
            for (int y = 0; y < maze_col; y++)
            {
                temp_char = maze[x,y];
                if (temp_char == '\\' || temp_char == '/' || temp_char == '-')
                { // This will match the head of manic when he is at crawling position and will move one step right
                    maze[x,y] = ' ';
                    Console.SetCursorPosition(y, x);
                Console.Write(' ');
                maze[x,y - 1] = temp_char;
                Console.SetCursorPosition(y - 1, x);
                Console.Write(temp_char);
                }
            }
        }
    }
    static void ClimbUp(int temp_x, int temp_y)
    {
        char PreviousItem1 = '|';
        char PreviousItem2 = '_';
        ladder_count++;
        ManicClimbing = "CLIMBING";
        if (ladder_count < 4)
        {
            step1 = "START";
        }
        else
        {
            step1 = "STOP";
        }
        if (ladder_count >= 4)
        {
            step2 = "START";
            // ManicClimbing = "CLIMBING";
        }
        else
        {
            step2 = "STOP";
        }
        if (step1 == "START")
        {
            ManicMoveLeft();

            if (ladder_count == 3)
            {
                isManicFrozen = "FROZEN";
                SetManicCurrentLocation();
                for (int temp_row = Manic_Current_Row; temp_row <= Manic_Current_Row + 2; temp_row++)
                {
                    maze[temp_row,Manic_Current_Col + 2] = PreviousItem1;
                    Console.SetCursorPosition(Manic_Current_Col + 2, temp_row);
                Console.Write(PreviousItem1);
                step1 = "STOP";
                }
            }
        }
        else if (step2 == "START")
        {
            ManicMoveUp();
            if (ladder_count > 4)
            {
                SetManicCurrentLocation();
                maze[Manic_Current_Row + 3,Manic_Current_Col] = '_';
                maze[Manic_Current_Row + 3,Manic_Current_Col + 1] = '_';
                Console.SetCursorPosition(Manic_Current_Col, Manic_Current_Row + 3);
            Console.Write('_');
            Console.SetCursorPosition(Manic_Current_Col + 1, Manic_Current_Row + 3);
            Console.Write('_');
            if (ladder_count == 9)
            {
                    ladder_count = 0;
                    UpdateManicLadderPositionUp(temp_x, temp_y); // Putting Manic to Upper side after removing manic from ladder
                    ManicClimbing = "NOT CLIMBING";
                    isManicFrozen = "NOT FROZEN";
            }
            }
        }
    }
    static void ManicClimb(int temp_x, int temp_y)
    {
        // This Function will climb manic at up or down position according to ladder position
        if (LadderPosition == "UP") // Manic will move Climb up to ladder
        {
            ClimbUp(temp_x, temp_y);
        }
        else if (LadderPosition == "DOWN")
        {
            ClimbDown(temp_x, temp_y);
        }
    }
   static string isClimbPosition()
    {                                     // This Function is used to check manic should climb up or climb down
        string temp_position = "NOTHING"; // By Default there is nothing means that ladder is not present near manic to climb up or climb down
        if (isClimbUpPossible())
        {
            temp_position = "UP"; // UP means ladder is set to move up
        }
        else if (isClimbDownPossible())
        {
            temp_position = "DOWN"; // DOWN means that ladder is set at position that manic can move down
        }
        return temp_position;
    }
    static string ManicClimbDownDirection()
    { // This Function is used to check wheather ladder is at right or left side of the manic
        string temp_status = "NOTHING";
        SetManicCurrentLocation();
        if (maze[Manic_Current_Row + 4,Manic_Current_Col - 1] == '_' || maze[Manic_Current_Row + 4,Manic_Current_Col - 2] == '_')
        {
            temp_status = "LEFT"; // ladder is at the left side of MAnic
        }
        else if (maze[Manic_Current_Row + 4,Manic_Current_Col + 2] == '_' || maze[Manic_Current_Row + 4,Manic_Current_Col + 3] == '_')
        {
            temp_status = "RIGHT"; // Ladder is at the right side of the Manic
        }
        return temp_status; // Returning the location of the ladder with respect to MAnic
    }
    static void ClimbDown(int temp_x, int temp_y)
    { // ClimbDown Function is used to Come DOwn From ladder
        char PreviousItem1 = '|';
        char PreviousItem2 = '_';
        isManicClimbing = "CLIMBING";
        ladder_count++;
        if (ladder_count < 4)
        {
            step1 = "START"; // Step1 is started in which manic will move Left to go down at next step
        }
        else
        {
            step1 = "STOP";
        }
        if (ladder_count >= 4)
        {
            step2 = "START";
        }
        else
        {
            step2 = "STOP";
        }
        if (ladder_count == 4)
        {
            isManicFrozen = "FROZEN"; // Manic is Frozen and cannot Move right or left when he is coming down from ladder
        }
        if (step1 == "START")
        {
            if (ManicClimbDownDirection() == "LEFT") // Instead of Functions there will be variables in future to avoid bugs
            {
                ManicMoveLeft();
            }
            else if (ManicClimbDownDirection() == "RIGHT")
            {
                ManicMoveRight();
            }
        }
        else if (step2 == "START")
        {
            ManicMoveDown();
            if (ladder_count >= 8)
            {
                SetManicCurrentLocation();
                maze[Manic_Current_Row - 1,Manic_Current_Col] = '_'; // This snippet is displaying __ when manic is going down
                Console.SetCursorPosition(Manic_Current_Col, Manic_Current_Row - 1);
            Console.Write('_');
            maze[Manic_Current_Row - 1,Manic_Current_Col + 1] = '_';
            Console.SetCursorPosition(Manic_Current_Col + 1, Manic_Current_Row - 1);
            Console.Write('_');
            }
            if (ladder_count == 10)
            {
                // Move Manic out of the ladder
                ladder_count = 0;
                CompleteBrokenLadder();
                UpdateManicLadderPositionDown(temp_x, temp_y);
                isManicClimbing = "NOT CLIMBING";
                isManicFrozen = "NOT FROZEN";
            }
        }
    }
    static void CompleteBrokenLadder()
    {
        // This Function will complete the ladder when manic will come out of the ladder
        SetManicCurrentLocation();
        int x = Manic_Current_Row;
        int y = Manic_Current_Col;
        maze[x,y] = '_';
        maze[x,y + 1] = '_';
        maze[x + 1,y] = '_';
        maze[x + 1,y + 1] = '_';
        Console.SetCursorPosition(y, x);
    Console.Write('_');
    Console.SetCursorPosition(y + 1, x);
    Console.Write('_');
    Console.SetCursorPosition(y, x + 1);
    Console.Write('_');
    Console.SetCursorPosition(y + 1, x + 1);
    Console.Write('_');
    }
    static string isMoveBoxPossible()
    {
        string temp = " ";
        // bool isFound = false;
        temp = ManicCrawlingFaceLocation();
        for (int x = 0; x < maze_row; x++)
        {
            for (int y = 0; y < maze_col; y++)
            {
                if (temp == "LEFT")
                {
                    if (maze[x,y] == '/')
                    {
                        if (maze[x,y - 1] == ')')
                        { // This will check wheather box is present at front of manic when he is at layed on the ground
                            return "LEFT";
                        }
                    }
                }
                else if (temp == "RIGHT")
                {
                    if (maze[x,y] == '\\')
                    {
                        if (maze[x,y + 1] == '(')
                        {
                            return "RIGHT";
                        }
                    }
                }
            }
        }
        return "NOT"; // THis shows thers is no box at right or left
    }
    static void PlaceManicInBox()
    {
        bool isFound = false;
        string temp = isMoveBoxPossible();
        for (int x = 0; x < maze_row; x++)
        {
            for (int y = 0; y < maze_col; y++)
            {
                if (temp == "RIGHT")
                {
                    if (maze[x,y] == '\\')
                    {
                        RemoveManic();
                        maze[x - 2,y + 5] = '/'; // This is printing Manic inside the box if box is at right side
                        maze[x - 2,y + 6] = '\\';
                        maze[x - 1,y + 5] = '\\';
                        maze[x - 1,y + 6] = '/';
                        maze[x,y + 5] = '/';
                        maze[x,y + 6] = '\\';
                        Console.SetCursorPosition(y + 5, x - 2);
                    Console.Write('/');
                    Console.SetCursorPosition(y + 6, x - 2);
                    Console.Write('\\');
                    Console.SetCursorPosition(y + 5, x - 1);
                    Console.Write('\\');
                    Console.SetCursorPosition(y + 6, x - 1);
                    Console.Write('/');
                    Console.SetCursorPosition(y + 5, x);
                    Console.Write('/');
                    Console.SetCursorPosition(y + 6, x);
                    Console.Write('\\');
                    isFound = true;
                    isManicInBox = "IN BOX";
                    isManicFrozen = "FROZEN";
                    isManicLayed = "NOT LAYED";
                    break;
                    }
                }
                else if (temp == "LEFT")
                {
                    if (maze[x,y] == '/')
                    {
                        RemoveManic();
                        maze[x - 2,y - 5] = '/'; // This is printing Manic inside the box if box is at right side
                        maze[x - 2,y - 6] = '\\';
                        maze[x - 1,y - 5] = '\\';
                        maze[x - 1,y - 6] = '/';
                        maze[x,y - 5] = '/';
                        maze[x,y - 6] = '\\';
                        Console.SetCursorPosition(y - 5, x - 2);
                    Console.Write('/');
                    Console.SetCursorPosition(y - 6, x - 2);
                    Console.Write('\\');
                    Console.SetCursorPosition(y - 5, x - 1);
                    Console.Write('\\');
                    Console.SetCursorPosition(y - 6, x - 1);
                    Console.Write('/');
                    Console.SetCursorPosition(y - 5, x);
                    Console.Write('/');
                    Console.SetCursorPosition(y - 6, x);
                    Console.Write('\\');
                    isFound = "true";
                    isManicInBox = "IN BOX";
                    isManicFrozen = "FROZEN";
                    isManicLayed = "NOT LAYED";
                    break;
                    }
                }
            }
            if(isFound)
            {
                break;
            }
            isManicFrozen = "NOT FROZEN";
        }
    }
    static void PlaceManicOutBox()
    {
        bool temp_flag = false;
        // THis Function will move manic out of the box
        for (int x = 0; x < maze_row; x++)
        {
            for (int y = 0; y < maze_col; y++)
            {
                if (maze[x,y] == '(')
                {
                    temp_flag = true;
                    RemoveManic();
                }
                if (temp_flag)
                {
                    maze[x + 2,y - 1] = '\\';
                    maze[x + 2,y - 2] = '/';
                    maze[x + 2,y - 3] = '-';
                    maze[x + 2,y - 4] = '-';
                    Console.SetCursorPosition(y - 1, x + 2);
                Console.Write('\\');
                Console.SetCursorPosition(y - 2, x + 2);
                Console.Write('/');
                Console.SetCursorPosition(y - 3, x + 2);
                Console.Write('-');
                Console.SetCursorPosition(y - 4, x + 2);
                Console.Write('-');
                break;
                }
            }
            if (temp_flag)
            {
                isManicLayed = "LAYED";
                isManicFrozen = "NOT FROZEN";
                isManicInBox = "NOT IN BOX";
                break;
            }
        }
    }
    static bool isManicStandUpPossible()
    {
        int temp_count = 0;
        for (int x = 0; x < maze_row; x++)
        {
            for (int y = 0; y < maze_col; y++)
            {
                if (maze[x,y] == '/')
                {
                    if (maze[x,y - 1] == ' ' && maze[x,y - 2] == ' ' && maze[x - 1,y - 1] == ' ' && maze[x - 1,y - 2] == ' ' && maze[x - 2,y - 1] == ' ' && maze[x - 2,y - 2] == ' ' && maze[x - 1,y] != ' ')
                    {
                        // This condition is used to check if manic can come out from narrow path
                        return true;
                    }
                }
            }
        }
        return false;
    }
    static void ManicStandUp()
    {
        bool isFound = false;
        if (isManicStandUpPossible())
        {
            for (int x = 0; x < maze_row; x++)
            {
                for (int y = 0; y < maze_col; y++)
                {
                    if (maze[x,y] == '/')
                    {
                        RemoveManic();
                        maze[x - 2,y - 2] = '/';
                        maze[x - 2,y - 1] = '\\';
                        maze[x - 1,y - 2] = '\\';
                        maze[x - 1,y - 1] = '/';
                        maze[x,y - 1] = '\\';
                        maze[x,y - 2] = '/';
                        Console.SetCursorPosition(y - 2, x - 2);
                    Console.Write('/');
                    Console.SetCursorPosition(y - 1, x - 2);
                    Console.Write('\\');
                    Console.SetCursorPosition(y - 2, x - 1);
                    Console.Write('\\');
                    Console.SetCursorPosition(y - 1, x - 1);
                    Console.Write('/');
                    Console.SetCursorPosition(y - 2, x);
                    Console.Write('/');
                    Console.SetCursorPosition(y - 1, x);
                    Console.Write('\\');
                    isFound = true;
                    break;
                    }
                }
                if (isFound)
                {
                    break;
                }
            }
        }
    }
    static bool isNextLevel()
    {
        if (LevelStatus == "LEVEL2")
        {
            return true;
        }
        return false;
    }
   static void ChangeMaze()
    {
        if (isNextLevel())
        {
            Console.Clear();
            maze_row = 42;
            maze_col = 107;
            loadMaze();
            printMaze();
            ManicFallingStatus = "NOT FALLING";
            isManicFrozen = "NOT FROZEN";
            isManicLayed = "NOT LAYED";
            isManicInBox = "NOT IN BOX";
        }
    }
    static void movePlate()
    {
        SetManicCurrentLocation();
        if (plateMoving == "RIGHT")
        {
            PlateMoveRight();
            if (maze[Manic_Current_Row + 3,Manic_Current_Col] == '=')
            {
                ManicMoveRight();
            }
        }
        else if (plateMoving == "LEFT")
        {
            PlateMoveLeft();
            if (maze[Manic_Current_Row + 3,Manic_Current_Col] == '=')
            {
                ManicMoveLeft();
            }
        }
    }
    static void PlateMoveLeft()
    {
        for (int temp_r = 0; temp_r < maze_row; temp_r++)
        {
            for (int temp_c = 0; temp_c < maze_col; temp_c++)
            {
                if (maze[temp_r,temp_c] == '=')
                {
                    if (maze[temp_r,temp_c - 1] == ' ')
                    {
                        maze[temp_r,temp_c] = ' ';
                        maze[temp_r,temp_c - 1] = '=';
                        Console.SetCursorPosition(temp_c, temp_r);
                    Console.Write(' ');
                    Console.SetCursorPosition(temp_c - 1, temp_r);
                    Console.Write('=');
                    }
                    else
                    {
                        plateMoving = "RIGHT";
                    }
                }
            }
        }
    }
   static void PlateMoveRight()
    {
        for (int temp_r = 0; temp_r < maze_row; temp_r++)
        {
            for (int temp_c = maze_col - 1; temp_c != 0; temp_c--)
            {
                if (maze[temp_r,temp_c] == '=')
                {
                    if (maze[temp_r,temp_c + 1] == ' ')
                    {
                        maze[temp_r,temp_c] = ' ';
                        maze[temp_r,temp_c + 1] = '=';
                        Console.SetCursorPosition(temp_c, temp_r);
                    Console.Write(' ');
                    Console.SetCursorPosition(temp_c + 1, temp_r);
                    Console.Write('=');
                    }
                    else
                    {
                        plateMoving = "LEFT";
                    }
                }
            }
        }
    }
    static int FireDirection()
    {
        srand(time(0));
        int result = 1 + (rand() % 105);
        return result;
    }
    static void generatefire()
    {
        int value = FireDirection(); // Getting value to be used for random column
        maze[1,value] = '&';        // generating fire at row 1
        Console.SetCursorPosition(value, 1);
    Console.Write('&');
    }
    static void MoveFire()
    {
        for (int x = maze_row - 1; x != 0; x--)
        {
            for (int y = 0; y < maze_col; y++)
            {
                if (maze[x,y] == '&')
                {
                    if (maze[x + 1,y] == ' ')
                    {
                        maze[x,y] = ' ';
                        maze[x + 1,y] = '&';
                        Console.SetCursorPosition(y, x);
                    Console.Write(' ');
                    Console.SetCursorPosition(y, x + 1);
                    Console.Write('&');
                    }
                    else if (maze[x + 1,y] == '/' || maze[x + 1,y] == '\\')
                    {
                        maze[x,y] = ' ';
                        Console.SetCursorPosition(y, x);
                    Console.Write(' ');
                    health = health - 5;
                    }
                    else
                    {
                        maze[x,y] = ' ';
                        Console.SetCursorPosition(y, x);
                    Console.Write(' ');
                    }
                }
            }
        }
    }
    static void CalculateScore()
    {
        int temp_count = 0;
        for (int x = 0; x < maze_row; x++)
        { // Identifying how many keys are left in maze
            for (int y = 0; y < maze_col; y++)
            {
                if (maze[x,y] == '!')
                {
                    temp_count++;
                }
            }
        }
        if (LevelStatus == "LEVEL1")
        {
            keys_captured = keysLevel1 - temp_count;
        }
        else
        {
            if (LevelStatus == "LEVEL2")
            {
                keys_captured = keysLevel2 - temp_count;
            }
        }
        if (LevelStatus == "LEVEL1")
        {
            score = 5 * keys_captured;
        }
        if (LevelStatus == "LEVEL2")
        {
            score = (5 * keys_captured) + (5 * keysLevel1); // It is understood that in level1 there was two key so score in level1 is 5*keysLevel1
        }
    }
    static void PrintScore()
    {
        Console.SetCursorPosition(12, 46);
    Console.Write("Score:\t{0}", score);
    }
    static void PrintHealth()
    {
        Console.SetCursorPosition(12, 48);
    Console.Write("HEALTH:\t   ");
    Console.SetCursorPosition(12, 48);
    Console.Write("HEALTH:\t{0}", health);
    }
    static void printInstrunctions()
    {
        string key;
        Console.Clear();
            Console.WriteLine("Use Right Arrow key to move Right");
    Console.WriteLine("Use Left Arrow Key to Move Left");
    Console.WriteLine("Use SpaceBar to Jump");
    Console.WriteLine("Use Numpad 0 to Climb up and come down from ladder");
    Console.WriteLine("Use Numpad 5 to Crawl");
    Console.WriteLine("press any key to continue");
    Console.ReadKey();
    Console.Clear();
    }
    static void GameOver()
    {
        Console.Clear();
        CleanScreen();
        Console.SetCursorPosition(10, 10);
    Console.WriteLine("******************************************");
    Console.WriteLine("*           Game is Finished You Win      *");
    Console.WriteLine("******************************************");
    }
    static void CleanScreen()
    {
        for (int x = 0; x < 58; x++)
        {
            for (int y = 0; y < 115; y++)
            {
                Console.SetCursorPosition(x, y);
            Console.Write(' ');
            }
        }
    }
}
}
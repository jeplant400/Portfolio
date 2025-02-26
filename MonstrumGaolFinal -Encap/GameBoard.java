import greenfoot.*;  // (World, Actor, GreenfootImage, Greenfoot and MouseInfo)

/**
 * GameBoard world where everything happens
 * 
 * @author James Plant
 * @version 12/12/2024
 */
public class GameBoard extends World
{
    private boolean[] playerIsHumanForPlayerIndex;
    
    private Tabletop tabletop;
    private Button button;
    //private Text text;
    
    
    private Text resultText;  
    private int playerScore = 0;  
    private int computerScore = 0;  
    private int playerBonusPoints =0;
    private int computerBonusPoints =0;
    private GreenfootImage scoreBoard;
    
    
    private Die die;
    private Space[] spaces;
    
    //revert to these to prevent loss in graphic quality
    
    //public static final int HEIGHT = 960;
    //public static final int WIDTH = 672;
    
    public static final int HEIGHT = 928;
    public static final int WIDTH = 576;
    
    
    private PlayerPiece[][] playerPieces;  
    
    private final int NUMBER_OF_PIECES_PER_PLAYER = 3;  // set this value here, once, then use the constant to get this value elsewhere in the class
    private final int DELAY_LENGTH = 60; 
    
    private int[] heroStartYCoords;                     // 1-D array of ints for storing the starting Y coordinates for the crabs (initialized in constructor)
    private int[] monsterStartYCoords;
    
    private Space[][] movementPathForPlayerIndex; 
    
    public int dieRollValue;  //was private                         // GameBoard "has-a" value of the most recent die roll
    private final int DIE_TEXT_VERTICAL_OFFSET = 50;
    
    private int state;
    
    public static final int BOARD_SETUP = 0;            // accessed by BOARD_SETUP within GameBoard; is public, so access using `Gameboard.BOARD_SETUP` elsewhere
    public static final int PLAYER1_ROLL_DIE = 1;       // accessed by PLAYER1_ROLL_DIE within GameBoard; is public, so access using `Gameboard.PLAYER1_ROLL_DIE` elsewhere
    public static final int PLAYER1_MOVE_HERO = 2;      // accessed by PLAYER1_MOVE_CRAB within GameBoard; is public, so access using `Gameboard.PLAYER1_MOVE_CRAB` elsewhere
    public static final int PLAYER2_ROLL_DIE = 3;       // accessed by PLAYER2_ROLL_DIE within GameBoard; is public, so access using `Gameboard.PLAYER2_ROLL_DIE` elsewhere
    public static final int PLAYER2_MOVE_MONSTER = 4;   // accessed by PLAYER2_MOVE_LOBSTER within GameBoard; is public, so access using `Gameboard.PLAYER2_MOVE_LOBSTER` elsewhere
    public static final int PLAYER1_WIN = 5;            // accessed by PLAYER1_WIN within GameBoard; is public, so access using `Gameboard.PLAYER1_WIN` elsewhere
    public static final int PLAYER2_WIN = 6;            // accessed by PLAYER2_WIN within GameBoard; is public, so access using `Gameboard.PLAYER2_WIN` elsewhere

    private boolean readyToExitState;                   // GameBoard "has-a" value for tracking whether or not we are ready to update the game's overall state
    
    private boolean onSafeSpaceSoRollAgain;             // GameBoard "has-a" value for tracking whether or not the current player just landed on a safe space
                                                        // (and therefore that player gets to roll again)
    
    private int[] goalCountForPlayerIndex;              // GameBoard "has-a" value for tracking the number of crabs (for player index 0) or lobsters (for player index 1) 
                                                        // that have moved into the corresponding player's goal (i.e., movement path index 7)
    private boolean buttonVisible;
    private boolean tabletopVisible;
                                                      
    //private GameManager referenceToGameManager;
    /**
     * Constructor for objects of the GameBoard
     * 
     */
    public GameBoard()
    {    
        
        super(WIDTH, HEIGHT, 1, false);
        Greenfoot.setWorld(new TitleScreen());
        playerIsHumanForPlayerIndex = new boolean[]{ true, false };
        
        spaces = new Space[20];
        playerPieces = new PlayerPiece[2][NUMBER_OF_PIECES_PER_PLAYER];
        
        //showText("Player Bonus: " + playerBonusPoints + playerScore , 250, 100);  
        //showText("Computer Bonus: " + computerBonusPoints + computerScore, 450, 100);
        state = BOARD_SETUP;
        
        updateBonusPoints();
        
        
        readyToExitState = false;
        onSafeSpaceSoRollAgain = false;
        
        goalCountForPlayerIndex = new int[2]; // set size of array to be 2 elements long 
        goalCountForPlayerIndex[0] = 0; // at start of game, no crabs in the goal
        goalCountForPlayerIndex[1] = 0;
        
        
        heroStartYCoords = new int[]{ 60, 120, 180 };   // NOTE: if using more or less than 3 pieces per player, you'll 
        monsterStartYCoords = new int[]{ 60, 120, 180 };  
        
        tabletopVisible = false;
        buttonVisible = false;
        prepare();
    }// end gameboard constructor
    
    
    
    /**
     * getter and setter list
     * 
     */
     public int getPlayerScore() {
        return playerScore;
    }// end getPlayerScore

    public void setPlayerScore(int score) {
        this.playerScore = score;
        updateBonusPoints();
    }// end setPlayerScore

    public int getComputerScore() {
        return computerScore;
    } // end getComputerScore

    public void setComputerScore(int score) {
        this.computerScore = score;
        updateBonusPoints();
    } // end setComputerScore

    public int getDieRollValue() {
        return dieRollValue;
    } // end getDieRollValue

    public void setDieRollValue(int dieRollValue) {
        this.dieRollValue = dieRollValue;
    } // end setDieRollValue

    public int getState() {
        return state;
    }// end getState

    public void setState(int state) {
        this.state = state;
    }// end setState

    public boolean isReadyToExitState() {
        return readyToExitState;
    }//end isReadyToExitState

    public void setReadyToExitState(boolean readyToExitState) {
        this.readyToExitState = readyToExitState;
    }// end setReadyToExitState

    public boolean isOnSafeSpaceSoRollAgain() {
        return onSafeSpaceSoRollAgain;
    }// isOnSafeSpaceSoRollAgain

    public void setOnSafeSpaceSoRollAgain(boolean onSafeSpaceSoRollAgain) {
        this.onSafeSpaceSoRollAgain = onSafeSpaceSoRollAgain;
    }// end setOnSafeSpaceSoRollAgain

    public boolean isButtonVisible() {
        return buttonVisible;
    }// end isButtonVisible

    public void setButtonVisible(boolean buttonVisible) {
        this.buttonVisible = buttonVisible;
    }//end setButtonVisible

    public boolean isTabletopVisible() {
        return tabletopVisible;
    }// end isTabletopVisible

    public void setTabletopVisible(boolean tabletopVisible) {
        this.tabletopVisible = tabletopVisible;
    }// end setTabletopVisible
    /**
     * sets up objects on the board
     * 
     */
    
    private void prepare()
    {
        /* board space */
        setBackground("background.png");
        
        
        for ( int spaceIndex = 0; spaceIndex < 8; spaceIndex++ )
        {
            if ( spaceIndex != 3 ) // if NOT the middle space (i.e., if NOT the safe space)...
            {
                spaces[spaceIndex] = new Space( false ); // is a stone space
            } 
            else // otherwise, if the middle space (i.e., if the safe space),,,
            {
                spaces[spaceIndex] = new Space( true ); // is a green space
            }
            addObject( spaces[spaceIndex], (int)(0.5 * WIDTH), 95 * spaceIndex + 200 ); // y = mx + b

        } // end for
        for ( int spaceIndex = 8; spaceIndex < 12; spaceIndex++ )
        {
            if ( spaceIndex != 8 ) // if NOT the middle space (i.e., if NOT the safe space)...
            {
                spaces[spaceIndex] = new Space( false ); // is a stone space
            } 
            else // otherwise, if the middle space (i.e., if the safe space),,,
            {
                spaces[spaceIndex] = new Space( true ); // is a green space
            }
            addObject( spaces[spaceIndex], (int)(0.358 * WIDTH), 95 * (spaceIndex-8) + 200 ); // y = mx + b
        
        }
        for ( int spaceIndex = 12; spaceIndex < 16; spaceIndex++ )
        {
            if ( spaceIndex != 12 ) // if NOT the middle space (i.e., if NOT the safe space)...
            {
                spaces[spaceIndex] = new Space( false ); // is a stone space
            } 
            else // otherwise, if the middle space (i.e., if the safe space),,,
            {
                spaces[spaceIndex] = new Space( true ); // is a green space
            }
            addObject( spaces[spaceIndex], (int)(0.643 * WIDTH), 95 * (spaceIndex-12) + 200 ); // y = mx + b
        
        }
        for ( int spaceIndex = 16; spaceIndex < 18; spaceIndex++ )
        {
            if ( spaceIndex != 16 ) // if NOT the middle space (i.e., if NOT the safe space)...
            {
                spaces[spaceIndex] = new Space( false ); // is a stone space
            } 
            else // otherwise, if the middle space (i.e., if the safe space),,,
            {
                spaces[spaceIndex] = new Space( true ); // is a green space
            }
            addObject( spaces[spaceIndex], (int)(0.643 * WIDTH), 95 * (spaceIndex-16) + 770 ); // y = mx + b
        
        }
        for ( int spaceIndex = 18; spaceIndex < 20; spaceIndex++ )
        {
            if ( spaceIndex != 18 ) // if NOT the middle space (i.e., if NOT the safe space)...
            {
                spaces[spaceIndex] = new Space( false ); // is a stone space
            } 
            else // otherwise, if the middle space (i.e., if the safe space),,,
            {
                spaces[spaceIndex] = new Space( true ); // is a green space
            }
            addObject( spaces[spaceIndex], (int)(0.358 * WIDTH), 95 * (spaceIndex-18) + 770 ); // y = mx + b
        
        }
        
        movementPathForPlayerIndex = new Space[2][20];
        
        movementPathForPlayerIndex[0][0] = spaces[11]; 
        movementPathForPlayerIndex[0][1] = spaces[10];  
        movementPathForPlayerIndex[0][2] = spaces[9]; 
        movementPathForPlayerIndex[0][3] = spaces[8]; 
        movementPathForPlayerIndex[0][4] = spaces[0]; 
        movementPathForPlayerIndex[0][5] = spaces[1]; 
        movementPathForPlayerIndex[0][6] = spaces[2]; 
        movementPathForPlayerIndex[0][7] = spaces[3]; 
        movementPathForPlayerIndex[0][8] = spaces[4]; 
        movementPathForPlayerIndex[0][9] = spaces[5]; 
        movementPathForPlayerIndex[0][10] = spaces[6]; 
        movementPathForPlayerIndex[0][11] = spaces[7]; 
        movementPathForPlayerIndex[0][12] = spaces[19]; 
        movementPathForPlayerIndex[0][13] = spaces[18]; 
        
        
        
        movementPathForPlayerIndex[1][0] = spaces[15]; 
        movementPathForPlayerIndex[1][1] = spaces[14]; 
        movementPathForPlayerIndex[1][2] = spaces[13];  
        movementPathForPlayerIndex[1][3] = spaces[12]; 
        movementPathForPlayerIndex[1][4] = spaces[0]; 
        movementPathForPlayerIndex[1][5] = spaces[1]; 
        movementPathForPlayerIndex[1][6] = spaces[2]; 
        movementPathForPlayerIndex[1][7] = spaces[3]; 
        movementPathForPlayerIndex[1][8] = spaces[4]; 
        movementPathForPlayerIndex[1][9] = spaces[5]; 
        movementPathForPlayerIndex[1][10] = spaces[6]; 
        movementPathForPlayerIndex[1][11] = spaces[7]; 
        movementPathForPlayerIndex[1][12] = spaces[17]; 
        movementPathForPlayerIndex[1][13] = spaces[16]; 
        
        
        for ( int heroIndex = 0; heroIndex < playerPieces[0].length; heroIndex++ )
        {
            int heroXcoord = (int)(0.2 * WIDTH);
            int heroYcoord = heroStartYCoords[heroIndex];
            playerPieces[0][heroIndex] = new PlayerPiece(0, heroXcoord, heroYcoord); 
            addObject( playerPieces[0][heroIndex], heroXcoord, heroYcoord );
        } // end for

        for ( int monsterIndex = 0; monsterIndex < playerPieces[1].length; monsterIndex++ )
        {
            int monsterXcoord = (int)(0.8 * WIDTH);
            int monsterYcoord = monsterStartYCoords[monsterIndex];
            playerPieces[1][monsterIndex] = new PlayerPiece(1, monsterXcoord, monsterYcoord );
            addObject( playerPieces[1][monsterIndex], monsterXcoord, monsterYcoord );
        } // end for
        
        die = new Die();
        addObject(die, 500, 500);
        
        state = PLAYER1_ROLL_DIE;
        /* place die */
        // basically all of this code was made by Dr. Canada
    }// end method prepare
    
    /**
     * sets up game states
     * 
     */
    
    
    public void act()
    {
        switch ( state ) {
            case PLAYER1_ROLL_DIE:
                determineDieRollValueForPlayerIndex(0);
                break; // break out of **switch** but if the game state isn't updated, we'll continue checking for die roll

            case PLAYER1_MOVE_HERO:
                determineWhichPiecesAreMoveableForPlayerIndex( 0 ); // remember, player "number" 1 (crabs) is player INDEX 0
                if ( !readyToExitState ) {
                    determineMoveForPlayerIndex(0);
                    return; // exits `act` method (and skips additional method calls for the current `act` method call)
                            // because player 1 (crabs, for player index 0) hasn't selected a piece to move yet
                } // end if
                
                // Note that the only way we end up here is if `readyToExitState` is `true` (for the current value of `state`)
                makeAllPiecesMoveableAgainForPlayerIndex(0);  
                updateGameStateAfterTurnForPlayerIndex(0);  
            
                // OPTIONAL method call, uncomment for debugging/diagnostic purposes 
                // logCurrentStateOfGameBoard( "At end of PLAYER1_MOVE_CRAB state" );
                
                break; // break out of **switch**

            case PLAYER2_ROLL_DIE:
                determineDieRollValueForPlayerIndex(1); 
                break;

            case PLAYER2_MOVE_MONSTER:
                determineWhichPiecesAreMoveableForPlayerIndex( 1 ); // remember, player "number" 2 (lobsters) is player INDEX 1
                if ( !readyToExitState ) {
                    determineMoveForPlayerIndex(1);
                    return; // exits `act` method (and skips additional method calls for the current `act` method call)
                            // because player 2 (lobsters, for player index 1) hasn't selected a piece to move yet

                } // end if ( !readyToExitState )
                
                // Note that the only way we end up here is if `readyToExitState` is `true` (for the current value of `state`)
                makeAllPiecesMoveableAgainForPlayerIndex(1);        
                updateGameStateAfterTurnForPlayerIndex(1);  
                
                // OPTIONAL method call, uncomment for debugging/diagnostic purposes 
                // logCurrentStateOfGameBoard( "At end of PLAYER2_MOVE_LOBSTER state" );
                
                break; // break out of **switch**

            case PLAYER1_WIN:
                showText( "\nPlayer 1\nWINS!!", die.getX(), die.getY() + DIE_TEXT_VERTICAL_OFFSET );
                Greenfoot.stop();
                break;

            case PLAYER2_WIN:
                showText( "\nPlayer 2\nWINS!!", die.getX(), die.getY() + DIE_TEXT_VERTICAL_OFFSET );
                Greenfoot.stop();
                break;

            default:
                break;
                //This is all Dr.Canada
        } // end switch
    } // end method act
    /**
     * Determines the die roll for the current player. If the player is human,
     * then the human player clicks on the die object to roll the die; otherwise,
     * the die is automatically rolled by the CPU.
     * 
     * @param playerIndex  the index of the player rolling the die
     */
    private void determineDieRollValueForPlayerIndex( int playerIndex )
    {
        // Determine player number (not index), for use in string expressions later in this method.
        // The expression on the right uses Java's ternary conditional operator, ?:, 
        // to produce a construct that is a more concise version of an if/else statement). 
        // Try asking a generative AI to rewrite this statement as an if/else construct.
        // Which is more readable to you? Can you see the correspondence between the more 
        // compact conditional expression and a logically equivalent if/else construct?
        String playerNumberString = ( playerIndex == 0 ? "1" : "2" );
        
        // NOTE: Using a "guard condition" to avoid the need for nesting if-statements
        // (In plain English, we are asking: "If the player is human AND we have NOT clicked on the die....")
        if ( playerIsHumanForPlayerIndex[playerIndex] && !Greenfoot.mouseClicked(die) )
        {
            showText( "\nPlayer " + playerNumberString + "\nclick to roll", die.getX(), die.getY() + DIE_TEXT_VERTICAL_OFFSET );
            return; // skip remaining statements and return to this method's caller
        } // end if
        
        dieRollValue = Greenfoot.getRandomNumber(4); // rolls a 0 to 3, inclusively
 
        
        
        showText( "\nPlayer " + playerNumberString + "\nrolls a " + dieRollValue, die.getX(), die.getY() + DIE_TEXT_VERTICAL_OFFSET );

        die.updateImage(dieRollValue);
        Greenfoot.delay(DELAY_LENGTH); // allow time to view on-screen message
        
        if ( dieRollValue == 0 ) 
        {
            showText( "\nSkipping\nturn...", die.getX(), die.getY() + DIE_TEXT_VERTICAL_OFFSET );
            Greenfoot.delay(DELAY_LENGTH); // allow time to view on-screen message
            state = (playerIndex == 0 ? PLAYER2_ROLL_DIE : PLAYER1_ROLL_DIE ); // ternary operator
        } 
        else {
            state = ( playerIndex == 0 ? PLAYER1_MOVE_HERO : PLAYER2_MOVE_MONSTER ); // ternary operator
        } // end INNER if/else
        // all Dr.Canada
    } // end method determineDieRollValueForPlayerIndex
    
    /**
     * Routine for determining which heroes are moveable
     * 
     * @param playerIndex   index of the player whose turn is currently active
     */
    private void determineWhichPiecesAreMoveableForPlayerIndex( int playerIndex ) {
        
        // first check to see which of this player's pieces are moveable 
        // for the given die roll value 
        for ( int playerPieceIndex = 0; playerPieceIndex < playerPieces[playerIndex].length; playerPieceIndex++ ) 
        {    
            // determine index of opposing player using simple arithmetic (no if-statement needed!)
            int opposingPlayerIndex = 1 - playerIndex; // if playerIndex = 1, opposingPlayerIndex = 1 - 1 = 0
                                                       // if playerIndex = 0, opposingPlayerIndex = 1 - 0 = 1
            
            playerPieces[playerIndex][playerPieceIndex].setMoveable( false ); // "default" state; update depending on conditions to be checked below

            // call a "getter" method to retrieve the player piece's current position on the game board 
            int currentPlayerPieceGameBoardLocationIndex = 
                playerPieces[playerIndex][playerPieceIndex].getGameBoardLocationIndex();
                
            // Here, we "look ahead" by die roll value to determine "target" array index for this piece
            // (TODO: consider changing the variable name to reflect the word "target")
            int playerPieceTargetGameBoardLocationIndex = currentPlayerPieceGameBoardLocationIndex + dieRollValue;
            
            // if the player piece is ALREADY currently in the goal zone, then it shouldn't be moveable
            if ( currentPlayerPieceGameBoardLocationIndex == 14 )/* was 7*/
            {
                playerPieces[playerIndex][playerPieceIndex].setMoveable( false );
                continue; // we know this piece is NOT moveable so we can skip the remaining
                          // statements in this iteration of the for loop and then
                          // proceed to the next iteration
            } // end if
            
            // if the target space IS the goal, then this piece IS moveable
            // and we call a "setter" method to update the target space location index 
            // (as an attribute of the PlayerPiece object itself) so that this value can be 
            // easily retrieved by later methods (such as `handleSelectedPieceForPlayerIndex`)
            if ( playerPieceTargetGameBoardLocationIndex == 14 ) /* was 7*/
            {
                System.out.println("attempting to set target space for piece moving off the board");
                playerPieces[playerIndex][playerPieceIndex].setMoveable( true );
                playerPieces[playerIndex][playerPieceIndex].setTargetGameBoardLocationIndex(playerPieceTargetGameBoardLocationIndex);
                continue; // we know this piece IS moveable AND we know it's NOT in the start zone,
                          // so we skip remaining statements in `for` loop 
                          // BUT we will continue w/next iteration so we can determine if any other crabs are moveable
            } // end if

            // if the target space for this piece is BEYOND the goal (i.e., if die roll is too high to exactly "land on" the goal)
            // then this piece is NOT moveable
            if (playerPieceTargetGameBoardLocationIndex > 14 )/* was 7 */ 
            {
                playerPieces[playerIndex][playerPieceIndex].setMoveable( false );
                continue; // we know this crab is NOT moveable, so we 
                          // skip remaining statements in `for` loop but continue w/next iteration
            } // end if
            
            // Check to see if the target space is occupied by one of the current player's pieces
            // ...if so, then we CANNOT move to that space
            
            if ( movementPathForPlayerIndex[playerIndex][ playerPieceTargetGameBoardLocationIndex ].isOccupiedByPieceForPlayerIndex(playerIndex) ) {
                playerPieces[playerIndex][playerPieceIndex].setMoveable( false );
                continue; // we know this crab is NOT moveable, so we 
                          // skip remaining statements in `for` loop but continue w/next iteration
            } // end if
            
            // Check to see if the target space meets both of these conditions:
            // 1) is occupied by one of the OPPOSING player's pieces
            // AND
            // 2) is a safe space
            // ...if BOTH conditions are true, then this piece CANNOT be moved to that space
            if ( movementPathForPlayerIndex[playerIndex][ playerPieceTargetGameBoardLocationIndex ].isOccupiedByPieceForPlayerIndex(opposingPlayerIndex)  
                 && movementPathForPlayerIndex[playerIndex][ playerPieceTargetGameBoardLocationIndex ].isSafeSpace() )
            {
                playerPieces[playerIndex][playerPieceIndex].setMoveable( false );
                continue; // we know this crab is NOT moveable, so we 
                          // skip remaining statements in `for` loop but continue w/next iteration
            } // end if
            
            // Check to see if the target space meets both of these conditions:
            // 1) is occupied by one of the CURRENT player's pieces
            // AND
            // 2) is a safe space
            // ...if BOTH conditions are true, then this piece CANNOT be moved to that space
            if ( movementPathForPlayerIndex[playerIndex][ playerPieceTargetGameBoardLocationIndex ].isOccupiedByPieceForPlayerIndex(playerIndex)  
                 && movementPathForPlayerIndex[playerIndex][ playerPieceTargetGameBoardLocationIndex ].isSafeSpace() )
            {
                playerPieces[playerIndex][playerPieceIndex].setMoveable( false );
                continue; // we know this crab is NOT moveable, so we 
                          // skip remaining statements in `for` loop but continue w/next iteration
            } // end if
            
            //!!!
            // Check to see if the target space meets both of these conditions:
            // 1) is occupied by one of the OPPOSING player's pieces
            // AND
            // 2) is NOT a safe space
            // ...if BOTH conditions are true, then this piece CAN be moved to that space
            if ( movementPathForPlayerIndex[playerIndex][ playerPieceTargetGameBoardLocationIndex ].isOccupiedByPieceForPlayerIndex(opposingPlayerIndex)  
                 && !movementPathForPlayerIndex[playerIndex][ playerPieceTargetGameBoardLocationIndex ].isSafeSpace() )
            {
                playerPieces[playerIndex][playerPieceIndex].setMoveable( true );
                playerPieces[playerIndex][playerPieceIndex].setTargetGameBoardLocationIndex(playerPieceTargetGameBoardLocationIndex);
                System.out.println( "PIECE CAPTURED" );
            
            
                /*
                Tabletop newTabletop = new Tabletop();
                
                addObject(newTabletop, getWidth() /2 , getHeight() /2 );
                
                
                addObject(new Button("Rock"), 200, 650);
                addObject(new Button("Paper"), 350, 650);
                addObject(new Button("Scissors"), 500, 650);
                
                    // Add result text
                // Add result text
                resultText = new Text("Choose your move!");
                addObject(resultText, 350, 500);
        
                // Initialize and display the score
                scoreBoard = new GreenfootImage(200, 50);
                scoreBoard.setColor(Color.BLACK);
                scoreBoard.setFont(new Font("Arial", 24));
                updateScoreBoard();  
                showText("", 0, 0);    
               */
              
                 
              
                /*
                if ( !tabletopVisible )
                {
                    tabletopHelp();    
                }
                */
                        //Greenfoot.setWorld(new RPS());
            } // end OUTER if
            
            // At this point in the code, we have used the earlier if-statements to determine that the target space
            // is NOT already occupied (by either player), so if the target space for this piece is a safe space, 
            // then this piece IS moveable
            if ( movementPathForPlayerIndex[playerIndex][ playerPieceTargetGameBoardLocationIndex ].isSafeSpace() ) 
            {
                playerPieces[playerIndex][playerPieceIndex].setMoveable( true );
                playerPieces[playerIndex][playerPieceIndex].setTargetGameBoardLocationIndex(playerPieceTargetGameBoardLocationIndex);
            } // end OUTER if

            // otherwise, if we made it this far, we assume that NOTHING is preventing this piece from being moveable
            playerPieces[playerIndex][playerPieceIndex].setMoveable( true );
            playerPieces[playerIndex][playerPieceIndex].setTargetGameBoardLocationIndex(playerPieceTargetGameBoardLocationIndex);
        } // end for
        // code from Dr. Canada excluding the commented out tabletop code
    } // end method determineWhichPiecesAreMoveableForPlayerIndex
    
    
    
    /**
     * this holds all the objects made for RPS
     * 
     */
    private void tabletopHelp()
    {
        //Tabletop newTabletop = new Tabletop();
        tabletop = new Tabletop();
        tabletopVisible = true;
               
        
        
                addObject(tabletop, getWidth() /2 , getHeight() /2 );
                
                
                addObject(new Button("Rock"), 200, 650);
                addObject(new Button("Paper"), 350, 650);
                addObject(new Button("Scissors"), 500, 650);
                
                   
             
                resultText = new Text("Choose your move!");
                addObject(resultText, 350, 500);
        
               
                scoreBoard = new GreenfootImage(200, 50);
                scoreBoard.setColor(Color.BLACK);
                scoreBoard.setFont(new Font("Arial", 24));
                updateScoreBoard();  
                showText("", 0, 0);    
        
    }//end tabletopHelp
    
    
    /**
     * updates scoreboard
     * 
     */
    
    
    private void updateScoreBoard() {
        scoreBoard.clear();  
        scoreBoard.drawString("Player: " + playerScore + "  Computer: " + computerScore, 10, 30);
        showText("Player: " + playerScore , 200, 300);  
        showText("Computer: " + computerScore, 500, 300);
    }//end updatescoreboard
    
    
    /**
     * score result keeper
     * 
     */
    
    
    public void setResult(String result) {
        if (result.startsWith("You Win")) {
            playerScore++;
        } else if (result.startsWith("You Lose")) {
            computerScore++;
        }
        updateScoreBoard();
        updateBonusPoints();

        if (playerScore >= 2) {
            showText("Heroes Win the Game!", 300, 150);
            Greenfoot.delay(10);
            removeTabletop();
        } else if (computerScore >= 2) {
            showText("Monsters Win the Game!", 300, 150);
            Greenfoot.delay(10);
            removeTabletop();
        }
    }// end setResult
    /**
     * bonus point keeper
     * 
     */
    
     private void updateBonusPoints() {
        playerBonusPoints += playerScore ;
        computerBonusPoints += computerScore ;
        
        showText("Player Bonus: " + playerBonusPoints, 250, 100);  
        showText("Computer Bonus: " + computerBonusPoints, 430, 100);
    }// end bonuspoints
    
    
    /**
     * removes tabletop when RPS is over
     * 
     */
    
    private void removeTabletop() 
    {
        removeObject(tabletop);
        for (Button currentButton : getObjects(Button.class))
        {
            removeObject(currentButton);
        }
        for (ComputerMove currentComputerMove : getObjects(ComputerMove.class))
        {
            removeObject(currentComputerMove);
        }
        for (Text currentText : getObjects(Text.class))
        {
            removeObject(currentText);
        }
        showText("" ,300, 150);
        showText("" ,200, 300);  
        showText("" ,500, 300);
        playerScore = 0;
        computerScore = 0;
        
        
        //removeObject(button);
        //removeObject(text);
        
        System.out.println( "REMOVING TABLETOP " );
        tabletopVisible = false;
        buttonVisible = false;
        //tabletop.setLocation( 500, 800 );
        // the for list was Dr. Canadas idea
    }//end removeTabletop
    
   
    /**
     * Determines which of the player's moveable pieces will actually be moved
     * 
     * @param playerIndex   the index of the player currently moving 
     */
    private void determineMoveForPlayerIndex( int playerIndex )
    {
        int countOfPlayerPiecesThatAreNotMoveable = 0; // OK as a local variable

        // First, check to see if there are actually any moves to make
        for ( int playerPieceIndex = 0; playerPieceIndex < NUMBER_OF_PIECES_PER_PLAYER; playerPieceIndex++ )
        {
            PlayerPiece currentPlayerPieceToCheck = playerPieces[playerIndex][playerPieceIndex];
            
            if ( !currentPlayerPieceToCheck.isMoveable() ) 
            {
                countOfPlayerPiecesThatAreNotMoveable++;   
                
                if ( countOfPlayerPiecesThatAreNotMoveable == NUMBER_OF_PIECES_PER_PLAYER )
                {
                    showText( "No moves!", die.getX(), die.getY() + DIE_TEXT_VERTICAL_OFFSET );
                    Greenfoot.delay(DELAY_LENGTH); 
                    readyToExitState = true;
                    return; // terminate method (skipping all statements below) and return to method caller
                } // end INNER if
                
            } // end OUTER if 
        }
        
        // If you've gotten this far, then there is at least one moveable piece,
        // so we loop again through the moveable pieces to see which will be moved
        for ( int playerPieceIndex = 0; playerPieceIndex < NUMBER_OF_PIECES_PER_PLAYER; playerPieceIndex++ )
        {
            PlayerPiece currentPlayerPieceToCheck = playerPieces[playerIndex][playerPieceIndex];
            
            if ( currentPlayerPieceToCheck.isMoveable() )
            {
                String playerPieceName = playerIndex == 0 ? "hero" : "monster";
                
                // Note use of ternary conditional expression to determine whether to include the singular 
                // literal String "space" or the plural literal String "spaces," depending on the value of the die roll
                showText( "\n\n\nSelect\n" + playerPieceName + "\nto move\n" + dieRollValue + (dieRollValue == 1 ? " space" : " spaces"), 
                    die.getX(), die.getY() + DIE_TEXT_VERTICAL_OFFSET );
                
                if ( playerIsHumanForPlayerIndex[playerIndex] && Greenfoot.mouseClicked(currentPlayerPieceToCheck) ) 
                {                                                                          
                    handleSelectedPieceForPlayerIndex( playerIndex, currentPlayerPieceToCheck );
                    readyToExitState = true; // now that a piece is selected (here, by the human player), the game will update its state 
                    return; // move has been made, so we exit the method early and return to method caller
    
                } // end if 
    
                // if we've gotten to this point in the code, then we allow the CPU to determine which piece to move
                // (Specifically, we'll use a random number generator to simulate a 30% chance of the 
                //  CPU "mouse-clicking" on THIS piece -- it's not "smart" AI, but it works well enough)
                if ( !playerIsHumanForPlayerIndex[playerIndex] && Greenfoot.getRandomNumber(100) < 30 ) 
                {
                    Greenfoot.delay(DELAY_LENGTH); // allows time to see which board pieces are moveable or not
                    
                    showText( "\nMoving\n" + playerPieceName + " " + (playerPieceIndex + 1), die.getX(), die.getY() + DIE_TEXT_VERTICAL_OFFSET );
                    Greenfoot.delay(DELAY_LENGTH); // allows time to view message
    
                    handleSelectedPieceForPlayerIndex( playerIndex, currentPlayerPieceToCheck );
                    
                    readyToExitState = true; // now that a piece is selected (here, by the CPU), the game will update its state 
                    return; // move has been made, so we exit the method early and return to method caller
      
                } // end INNER if 
            } // end OUTER if
        } // end for
        //Dr. Canada code
    } // end method determineMoveForPlayerIndex

    /**
     * Updates the given player piece's location index along the movement path for the given playerIndex
     * 
     * @param playerIndex           the index of the player moving the selected piece
     * @param selectedPlayerPiece   a reference to the player's selected piece
     */
    private void handleSelectedPieceForPlayerIndex( int playerIndex, PlayerPiece selectedPlayerPiece )
    {
        if ( selectedPlayerPiece.getTargetGameBoardLocationIndex() >= 0 
              && 
             selectedPlayerPiece.getTargetGameBoardLocationIndex() < 14 )/* was 7, was 19 */ 
        {
            moveSelectedPieceOntoTargetSpaceForPlayerIndex( playerIndex, selectedPlayerPiece );
        } 
        else if ( selectedPlayerPiece.getTargetGameBoardLocationIndex() == 14 )/* was 7, was 19 */ { 
            moveSelectedPieceIntoGoalZoneForPlayerIndex( playerIndex, selectedPlayerPiece );
        } // end if/else

        // since we have now moved the player piece's location on the screen, let's first update
        // the status of the Space it is LEAVING so that it is NO LONGER OCCUPIED by that player...
        if ( selectedPlayerPiece.getGameBoardLocationIndex() >= 0 ) 
        {
            movementPathForPlayerIndex[ playerIndex ][ selectedPlayerPiece.getGameBoardLocationIndex() ].setOccupiedByPieceForPlayerIndex(playerIndex, false);
        } // end if
        
        // ...and finally, we UPDATE the selected player piece's  
        //    Space location to be whatever its TARGET location is 
        selectedPlayerPiece.setGameBoardLocationIndex( selectedPlayerPiece.getTargetGameBoardLocationIndex() );
        //Dr. Canada code
    } // end method handleSelectedPieceForPlayerIndex
    
    /**
     * "Helper" method (called by handleSelectedPieceForPlayerIndex) for moving a
     * piece into a target space that is NOT the goal
     * 
     * @param playerIndex           the index of the player moving a piece into the piece's target space 
     * @param selectedPlayerPiece   a reference to the player's selected piece
     */
    private void moveSelectedPieceOntoTargetSpaceForPlayerIndex( int playerIndex, PlayerPiece selectedPlayerPiece )
    {
        // determine index of opposing player using simple arithmetic (no if-statement needed!)
        int opposingPlayerIndex = 1 - playerIndex; // if playerIndex = 1, opposingPlayerIndex = 1 - 1 = 0
                                                   // if playerIndex = 0, opposingPlayerIndex = 1 - 0 = 1
        
        // move the playerPiece SPRITE to its new X- and Y- locations on the screen
        selectedPlayerPiece.setLocation( 
            movementPathForPlayerIndex[ playerIndex ][ selectedPlayerPiece.getTargetGameBoardLocationIndex() ].getX(), 
            movementPathForPlayerIndex[ playerIndex ][ selectedPlayerPiece.getTargetGameBoardLocationIndex() ].getY() );

        // update the "occupied" state for the target space where the selected player piece is being moved
        movementPathForPlayerIndex[ playerIndex ][ selectedPlayerPiece.getTargetGameBoardLocationIndex() ].setOccupiedByPieceForPlayerIndex(playerIndex, true);

        // if the target space is a safe space, mark for rolling again when the turn is over
        if ( movementPathForPlayerIndex[ playerIndex ][ selectedPlayerPiece.getTargetGameBoardLocationIndex() ].isSafeSpace() )
        {
            onSafeSpaceSoRollAgain = true;
        } // end if
        //!!!
        // if this space is occupied by a piece belonging to the OPPOSING player, 
        // move the OPPOSING playerPiece at this space back to the beginning
        
        
        if ( movementPathForPlayerIndex[ playerIndex ][ selectedPlayerPiece.getTargetGameBoardLocationIndex() ].isOccupiedByPieceForPlayerIndex(opposingPlayerIndex) ) 
        {
            resetCapturedPieceOnBoardAndReplaceWithSelectedPieceForPlayerIndex( playerIndex, selectedPlayerPiece );
        } // end if
        
        /*
                if (playerScore >= 2 && movementPathForPlayerIndex[0][selectedPlayerPiece.getTargetGameBoardLocationIndex()].isOccupiedByPieceForPlayerIndex(opposingPlayerIndex)) {
            resetCapturedPieceOnBoardAndReplaceWithSelectedPieceForPlayerIndex(0, selectedPlayerPiece);
        }
        
                if (computerScore >= 2 && movementPathForPlayerIndex[1][selectedPlayerPiece.getTargetGameBoardLocationIndex()].isOccupiedByPieceForPlayerIndex(opposingPlayerIndex)) {
            resetCapturedPieceOnBoardAndReplaceWithSelectedPieceForPlayerIndex(1, selectedPlayerPiece);
        }
        */
       
       
        //Dr. Canada code
    } // end method moveSelectedPieceOntoTargetSpaceForPlayerIndex
    
    /**
     * "Helper" method (here called by `moveSelectedPieceOntoTargetSpaceForPlayerIndex`) 
     * for handling the capture of an opposing player's piece 
     * 
     * @param playerIndex           the index of the player moving a piece into that piece's target space 
     * @param selectedPlayerPiece   a reference to the player's selected piece
     */
    private void resetCapturedPieceOnBoardAndReplaceWithSelectedPieceForPlayerIndex( int playerIndex, PlayerPiece selectedPlayerPiece )
    {
        // determine index of opposing player using simple arithmetic (no if-statement needed!)
        int opposingPlayerIndex = 1 - playerIndex; // if playerIndex = 1, opposingPlayerIndex = 1 - 1 = 0
                                                   // if playerIndex = 0, opposingPlayerIndex = 1 - 0 = 1
                                                   
        for ( PlayerPiece currentOpponentPieceToCheck : playerPieces[opposingPlayerIndex] ) {
            
            // if the captured opponent's piece is located along its board space movement path 
            // (index 0 to 6) then move it back to location "index" -1 (starting zone)
            // AND also move its SPRITE back to its original X- and Y-coordinate locations in the world
            
            if (currentOpponentPieceToCheck.getGameBoardLocationIndex() == -1 
                 ||
                currentOpponentPieceToCheck.getGameBoardLocationIndex() == 14) 
            {
                continue; //skip remaining statements in the current loop iteration but allow the loop to continue. 
            
            }//end if
            if ( !tabletopVisible )
                {
                    tabletopHelp();    
            }
            if ( movementPathForPlayerIndex[opposingPlayerIndex][currentOpponentPieceToCheck.getGameBoardLocationIndex()] 
                 == movementPathForPlayerIndex[playerIndex][selectedPlayerPiece.getTargetGameBoardLocationIndex()] ) 
            {
                //!!! CAPTURE ISSUE MAY BE HERE
                currentOpponentPieceToCheck.setLocation( currentOpponentPieceToCheck.getOriginalXcoord(), currentOpponentPieceToCheck.getOriginalYcoord() );
                
                currentOpponentPieceToCheck.setGameBoardLocationIndex(-1); // reset piece's location index back to 
                                                                           // starting zone (location "index" -1)
                
                // at the current player's TARGET location index along the movement path, update the state of that Space
                // so that it is NO LONGER OCCUPIED by the (captured) opposing player's piece
                movementPathForPlayerIndex[playerIndex][selectedPlayerPiece.getTargetGameBoardLocationIndex()].setOccupiedByPieceForPlayerIndex(opposingPlayerIndex, false);

            } // end if
        } // end for    
        //Dr. Canada code
    } // end method resetCapturedPieceOnBoardAndReplaceWithSelectedPieceForPlayerIndex
  
    /**
     * "Helper" method (here called by handleSelectedPieceForPlayerIndex) to move 
     * the selected playerPiece's SPRITE into the goal zone.
     * Note that this only moves the player piece's SPRITE; the player piece's 
     * board location index is updated elsewhere (can you figure out where?)
     * 
     * Actual X- and Y-coordinates of each player's piece in the goal zone are each  
     * computed as a linear function of how many of that player's pieces are already 
     * in the goal zone (i.e., goalCountForPlayer[playerIndex] )
     * 
     * @param playerIndex           the index of the player moving a piece into the goal zone
     * @param selectedPlayerPiece   a reference to the player's selected piece
     */
    private void moveSelectedPieceIntoGoalZoneForPlayerIndex( int playerIndex, PlayerPiece selectedPlayerPiece )
    {
        if ( playerIndex == 0 ) // if it's a crab...
        {
            selectedPlayerPiece.setLocation( (int)((0.75 + 0.05*goalCountForPlayerIndex[0]) * WIDTH), 20*goalCountForPlayerIndex[0] + 60 );
        } 
        else { // otherwise, if it's a lobster...
            selectedPlayerPiece.setLocation( (int)((0.125 + 0.05*goalCountForPlayerIndex[1]) * WIDTH), HEIGHT - 100 + 20 * goalCountForPlayerIndex[1] );
        } // end INNER if/else    
        //Dr. Canada code
    } // end method moveSelectedPieceIntoGoalZoneForPlayerIndex
    
    /**
     * "Turns on" (makes moveable) all of pieces for the given player (specified by `playerIndex`) 
     * at the conclusion of that player's turn
     * 
     * @param playerIndex   the index of the player that is completing their turn
     */
    private void makeAllPiecesMoveableAgainForPlayerIndex( int playerIndex )
    {
        for ( PlayerPiece currentPlayerPieceToCheck : playerPieces[playerIndex] )
        {
            currentPlayerPieceToCheck.setMoveable(true);
        } // end for
        //Dr. Canada code
    } // end method makeAllCrabsMoveableAgain
    
    /**
     * Updates the game state (and checks for a possible win condition) after the player
     * (specified by `playerIndex`) has just completed their turn
     * 
     * @param playerIndex   the index of the player that has just completed their turn
     */
    private void updateGameStateAfterTurnForPlayerIndex( int playerIndex )
    {
        switch ( playerIndex ) {
            case 0: // remember, playerIndex 0 means player NUMBER 1...
                updateGoalCountForPlayerIndex(0);
                updateBonusPoints();

                if ( goalCountForPlayerIndex[0] == NUMBER_OF_PIECES_PER_PLAYER || playerBonusPoints >= 20 ) {
                    state = PLAYER1_WIN; // update state for next `act` method call
                } 
               
                else if (onSafeSpaceSoRollAgain) {
                    showText( "\nPlayer 1\nrolls again!", die.getX(), die.getY() + DIE_TEXT_VERTICAL_OFFSET );
                    onSafeSpaceSoRollAgain = false; // reset for next turn
                    Greenfoot.delay(DELAY_LENGTH);
                    readyToExitState = false; // reset for next turn
                    state = PLAYER1_ROLL_DIE; // update state for next `act` method call
        
                } 
                else { 
                    // player 1 (index 0)'s turn is finished, so update game state for player 2's turn
                    showText( "\nPlayer 2\nup next", die.getX(), die.getY() + DIE_TEXT_VERTICAL_OFFSET );
                    Greenfoot.delay(DELAY_LENGTH);
                    readyToExitState = false; // reset for next turn
                    state = PLAYER2_ROLL_DIE; // update state for next `act` method call
        
                } // end multi-way if/else
                break;
            
            case 1: // remember, playerIndex 1 means player NUMBER 2...
                updateGoalCountForPlayerIndex(1);
                updateBonusPoints();
        
                if ( goalCountForPlayerIndex[1] == NUMBER_OF_PIECES_PER_PLAYER || (computerBonusPoints == 20)) 
                {
                    state = PLAYER2_WIN; // update state for next `act` method call
                } 
                
                else if (onSafeSpaceSoRollAgain) {
                    showText( "\nPlayer 2\nrolls again!", die.getX(), die.getY() + DIE_TEXT_VERTICAL_OFFSET );
                    onSafeSpaceSoRollAgain = false; // reset for next turn
                    Greenfoot.delay(DELAY_LENGTH);
                    readyToExitState = false; // reset for next turn
                    state = PLAYER2_ROLL_DIE; // update state for next `act` method call
        
                } 
                else { 
                    // player 1 (index 0)'s turn is finished, so update game state for player 2's turn
                    showText( "\nPlayer 1\nup next", die.getX(), die.getY() + DIE_TEXT_VERTICAL_OFFSET );
                    Greenfoot.delay(DELAY_LENGTH);
                    readyToExitState = false; // reset for next turn
                    state = PLAYER1_ROLL_DIE; // update state for next `act` method call
        
                }
                break;
                
            default:
                break;
                
        } // end switch
        //Dr. Canada code
    } // end method updateGameStateAfterTurnForPlayerIndex
    
    /**
     * For the given playerIndex, checks to see how many of that player's pieces have been 
     * moved into that goal zone at the end of that player's movement path
     * 
     * @param playerIndex   the index of the player whose pieces are being checked to see if they're in the goal zone
     */
    public void updateGoalCountForPlayerIndex( int playerIndex )
    {
        goalCountForPlayerIndex[playerIndex] = 0; // resetting for purpose of using 
                                                  // loop to re-compute goal count
        
        /* 
         * NOTE: An "enhanced for" loop works similarly to a counter-controlled
         *       for loop, but no control variable is used to iterate over the
         *       specified elements. 
         *       
         *       One might read the loop header below in plain English as:
         *       "For each playerPiece in playerPieces[playerIndex]..."
         */        
        for ( PlayerPiece currentPlayerPieceToCheck : playerPieces[playerIndex] )
        {
            if ( currentPlayerPieceToCheck.getGameBoardLocationIndex() == 14 )//was 7 
            {
                goalCountForPlayerIndex[playerIndex]++;
            } // end if
        } // end enhanced for loop 
        //Dr. Canada code
    } // end method updateGoalCountForPlayerIndex
    
    /**
     * A diagnostic method for logging the current state of the GameBoard to the console
     * (Feel free to modify this however you like, and consider using System.out.println elsewhere
     *  in your code to display similar "diagnostic messages" or "debug messages")
     * 
     * @param headerMessage     a message to help the reader know when this method was actually called 
     */
    public void logCurrentStateOfGameBoard( String headerMessage )
    {
        System.out.println( "----------------------------------------------------" );
        System.out.println( headerMessage );
        System.out.println( "----------------------------------------------------" );
        System.out.println( "Current states of all player 1 pieces (heros):");
        for ( int heroIndex = 0; heroIndex < NUMBER_OF_PIECES_PER_PLAYER; heroIndex++ )
        {
            System.out.println( "playerPieces[0]["+heroIndex+"].getGameBoardLocationIndex() = " 
                                    + playerPieces[0][heroIndex].getGameBoardLocationIndex() );
        } // end for
        
        System.out.println(); // blank line for readability
        
        System.out.println( "Current states of all player 2 pieces (monsters):");
        for ( int monsterIndex = 0; monsterIndex < NUMBER_OF_PIECES_PER_PLAYER; monsterIndex++ )
        {
            System.out.println( "playerPieces[1]["+monsterIndex+"].getGameBoardLocationIndex() = " 
                                    + playerPieces[1][monsterIndex].getGameBoardLocationIndex() );
        } // end for
        
        System.out.println(); // blank line for readability
        
        System.out.println( "Current states of all spaces on GameBoard:");
        for ( int spaceIndex = 0; spaceIndex < 7; spaceIndex++ )
        {
            System.out.println( "spaces["+spaceIndex+"].isOccupiedByPieceForPlayerIndex(0) = " 
                                    + spaces[spaceIndex].isOccupiedByPieceForPlayerIndex(0) );
            System.out.println( "spaces["+spaceIndex+"].isOccupiedByPieceForPlayerIndex(1) = " 
                                    + spaces[spaceIndex].isOccupiedByPieceForPlayerIndex(1) ); 
        } // end for
        
        System.out.println(); // blank line for readability
        //Dr. Canada code
    } // end method logCurrentStateOfGameBoard
    
}


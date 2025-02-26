import greenfoot.*;  // (World, Actor, GreenfootImage, Greenfoot and MouseInfo)

/**
 * The PlayerPiece class is all team pieces
 *
 *baesd on Dr.Canada code
 */

public class PlayerPiece extends Actor {

    private boolean moveable;
    private int originalXcoord;
    private int originalYcoord;
    private int gameBoardLocationIndex;
    private int targetGameBoardLocationIndex;
    private int playerIndex;

    /**
     * Constructor for the PlayerPiece.
     *
     * 
     * 
     * 
     */
    public PlayerPiece(int playerIndex, int originalXcoord, int originalYcoord) {
        this.playerIndex = playerIndex;
        this.originalXcoord = originalXcoord;
        this.originalYcoord = originalYcoord;

        
        this.moveable = true;
        this.gameBoardLocationIndex = -1;
        this.targetGameBoardLocationIndex = -1;

        // Set the image based on player index
        if (this.playerIndex == 0) {
            setImage(new GreenfootImage("HeroPiece.png"));
        } else {
            setImage(new GreenfootImage("MonsterPiece.png"));
        }

        // Scale image
        scaleImage(2);
    }//end constructor PlayerPiece



    /**
     * Updates the transparency of the piece.
     */
    public void act() {
        if (moveable) {
            getImage().setTransparency(255); 
        } else {
            getImage().setTransparency(128); 
        }
    }//end act

    /**
     * Scales the image of the piece.
     *
     * 
     */
    private void scaleImage(int scaleFactor) {
        GreenfootImage myImage = getImage();
        if (myImage != null) {
            int newWidth = myImage.getWidth() * scaleFactor;
            int newHeight = myImage.getHeight() * scaleFactor;
            myImage.scale(newWidth, newHeight);
        }
    }//end scaleImage

    /**
     * Getter for the moveable state.
     *
     */
    public boolean isMoveable() {
        return moveable;
    }//isMoveable

    /**
     * Setter for the moveable state.
     *
     */
    public void setMoveable(boolean moveable) {
        this.moveable = moveable;
    }//end setMoveable

    /**
     * Getter for the original X-coordinate.
     *
     */
    public int getOriginalXcoord() {
        return originalXcoord;
    }//end getOriginalXcoord

    /**
     * Getter for the original Y-coordinate.
     *
     */
    public int getOriginalYcoord() {
        return originalYcoord;
    }// end getOriginalYcoord

    /**
     * Getter for the current game board location index.
     *
     */
    public int getGameBoardLocationIndex() {
        return gameBoardLocationIndex;
    }// end getGameBoardLocationIndex

    /**
     * Setter for the game board location index.
     *
     */
    public void setGameBoardLocationIndex(int gameBoardLocationIndex) {
        this.gameBoardLocationIndex = gameBoardLocationIndex;
    }//end setGameBoardLocationIndex

    /**
     * Getter for the target game board location index.
     *
     */
    public int getTargetGameBoardLocationIndex() {
        return targetGameBoardLocationIndex;
    }//getTargetGameBoardLocationIndex

    /**
     * Setter for the target game board location index.
     *
     */
    public void setTargetGameBoardLocationIndex(int targetGameBoardLocationIndex) {
        this.targetGameBoardLocationIndex = targetGameBoardLocationIndex;
    }//setTargetGameBoardLocationIndex

    /**
     * Getter for the player index.
     *
     */
    public int getPlayerIndex() {
        return playerIndex;
    }//getPlayerIndex

    /**
     * Setter for the player index.
     *
     */
    public void setPlayerIndex(int playerIndex) {
        this.playerIndex = playerIndex;
    }//setPlayerIndex
}
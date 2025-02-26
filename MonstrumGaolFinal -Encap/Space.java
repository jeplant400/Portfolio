import greenfoot.*;  // (World, Actor, GreenfootImage, Greenfoot and MouseInfo)

/**
 * A space on the game board.
 *
 * based on Dr. Canada code
 */
public class Space extends Actor {

    private Die die;
    private boolean[] occupiedByPieceForPlayerIndex;
    private boolean safeSpace;

    /**
     * constructor for the Space class.
     * 
     */
    public Space() {
        scaleImage();
    }//end constructor space

    /**
     * Constructor for creating a Space with a specific safety state.
     *
     */
    public Space(boolean safeSpace) {
        this.occupiedByPieceForPlayerIndex = new boolean[]{false, false};
        this.safeSpace = safeSpace;

        if (this.safeSpace) {
            setImage(new GreenfootImage("DoubleTileFinal.png"));
        } else {
            setImage(new GreenfootImage("brickstile32x32.png"));
        }

        scaleImage();
    }//end constructor Space

    /**
     * Scales the image of the space.
     */
    private void scaleImage() {
        GreenfootImage myImage = getImage();
        if (myImage != null) {
            int myNewHeight = myImage.getHeight() * 3;
            int myNewWidth = myImage.getWidth() * 3;
            myImage.scale(myNewWidth, myNewHeight);
        }
    }//end scaleImage

    /**
     * Getter for the occupied state of the space for a specific player index.
     */
    public boolean isOccupiedByPieceForPlayerIndex(int playerIndex) {
        return occupiedByPieceForPlayerIndex[playerIndex];
    }//end isOccupiedByPieceForPlayerIndex

    /**
     * Setter for the occupied state of the space for a specific player index.
     *
     */
    public void setOccupiedByPieceForPlayerIndex(int playerIndex, boolean occupied) {
        this.occupiedByPieceForPlayerIndex[playerIndex] = occupied;
    }//end setOccupiedByPieceForPlayerIndex

    /**
     * Getter for the safe space state.
     *
     */
    public boolean isSafeSpace() {
        return safeSpace;
    }//end isSafeSpace

    /**
     * Setter for the safe space state.
     *
     */
    public void setSafeSpace(boolean safeSpace) {
        this.safeSpace = safeSpace;

        if (this.safeSpace) {
            setImage(new GreenfootImage("DoubleTileFinal.png"));
        } else {
            setImage(new GreenfootImage("brickstile32x32.png"));
        }

        scaleImage();
    }//end setSafeSpace
}

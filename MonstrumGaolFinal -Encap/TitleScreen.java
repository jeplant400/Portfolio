import greenfoot.*;

/**
 * This is the home title screen.
 *
 * 
 */
public class TitleScreen extends World {

    private GreenfootImage[] frames; // Array to store animation frames
    private int currentFrame;       // Tracks the current frame
    private int frameCounter;       // Controls timing for frame switching
    private int frameDelay;         // Adjusts animation speed (lower = faster)

    /**
     * Constructor for the TitleScreen class.
     * Sets up the world size, adds the title, and prepares the animation.
     */
    public TitleScreen() {
        super(720, 480, 1);
        this.frames = null;
        this.currentFrame = 0;
        this.frameCounter = 0;
        this.frameDelay = 5;

        Title title = new Title();
        addObject(title, getWidth() / 2, getHeight() / 4);
        prepareAnimation();
    }// end constructor

    /**
     * Prepares the animation frames for the title screen.
     */
    private void prepareAnimation() {
        frames = new GreenfootImage[7];
        int scaledWidth = 720;
        int scaledHeight = 480;

        for (int i = 0; i < frames.length; i++) {
            frames[i] = new GreenfootImage("frame" + (i + 1) + ".png"); 
            frames[i].scale(scaledWidth, scaledHeight); 
        }

        setBackground(frames[0]);
    }// end prepareAnimation

    /**
     * Called on every frame update to animate the title screen and check for user input.
     */
    public void act() {
        animateTitleScreen();
        checkForKeyPress();
    }//end act

    /**
     * Animates the title screen.
     */
    private void animateTitleScreen() {
        frameCounter++;
        if (frameCounter >= frameDelay) {
            frameCounter = 0;
            currentFrame = (currentFrame + 1) % frames.length;
            setBackground(frames[currentFrame]);
        }
    }// end animateTitleScreen

    /**
     * Checks if the space key is pressed.
     */
    private void checkForKeyPress() {
        if (Greenfoot.isKeyDown("space")) {
            Greenfoot.setWorld(new GameBoard());
        }
    }//end checkForKeypress

    /**
     * Getter for the current frame index.
     *
     * 
     */
    public int getCurrentFrame() {
        return currentFrame;
    }//end getCurrentFrame

    /**
     * Setter for the current frame index.
     *
     * 
     */
    public void setCurrentFrame(int currentFrame) {
        this.currentFrame = currentFrame;
    }//end setcurrentframe

    /**
     * Getter for the frame delay.
     *
     * 
     */
    public int getFrameDelay() {
        return frameDelay;
    }// end getFrameDelay

    /**
     * Setter for the frame delay.
     *
     * 
     */
    public void setFrameDelay(int frameDelay) {
        this.frameDelay = frameDelay;
    }// end setFramedelay
}

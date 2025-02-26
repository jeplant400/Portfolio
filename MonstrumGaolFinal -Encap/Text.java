import greenfoot.*;

/**
 * Displayed text.
 *
 */
public class Text extends Actor {

    private GreenfootImage image;

    /**
     * Constructor for the Text class.
     *
     */
    public Text(String message) {
        setText(message);
    }// end constructor text

    /**
     * Updates the text message displayed by this actor.
     *
     */
    public void setText(String message) {
        image = new GreenfootImage(message, 24, Color.BLACK, Color.WHITE);
        setImage(image);
    }// end setText

    /**
     * Getter for the current text image.
     *
     */
    public GreenfootImage getImageText() {
        return image;
    }// end getImageText
}

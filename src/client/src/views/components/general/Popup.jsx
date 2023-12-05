import PopupThread from "./popupThread";
import ThreadInput from "./threadInput";
const Popup = ({user, setOpenPopup, popupFeedThread, text, setText, postThread}) => {
    return (
        <div className="popup">
            <p onClick={() => setOpenPopup(false)}>X</p>
            {popupFeedThread?.map(_popupFeedThread =>
                <PopupThread
                    key={_popupFeedThread.id}
                    popupFeedThread={popupFeedThread}
                />
            )}
            <ThreadInput
                user={user}
                text={text}
                setText={setText}
                postThread={postThread}
            />
        </div>
    );
}

export default Popup;

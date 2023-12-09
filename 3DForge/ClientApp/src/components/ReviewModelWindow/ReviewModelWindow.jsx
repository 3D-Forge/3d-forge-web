import React from "react";
import cl from './.module.css';

const useRefDimensions = (ref) => {
    const [dimensions, setDimensions] = React.useState({ width: 1, height: 2 });

    function WindowResizeEvent() {
        if (ref.current) {
            const boundingRect = ref.current.getBoundingClientRect();
            setDimensions({ width: boundingRect.width, height: boundingRect.height });
        }
    }

    React.useEffect(() => {
        if (ref.current) {
            const boundingRect = ref.current.getBoundingClientRect();
            setDimensions({ width: boundingRect.width, height: boundingRect.height });
        }

        window.addEventListener('resize', WindowResizeEvent);

        return () => {
            window.removeEventListener('resize', WindowResizeEvent);
        };
    }, [ref])
    return dimensions;
}

const ReviewModelWindow = ({ visible = false, reviewingModelId = null, onClose = null }) => {
    const modalWindowRef = React.useRef();

    const modalWindowDimensions = useRefDimensions(modalWindowRef);

    return (
        <div className={cl.model_review_window_background} style={{ display: visible ? 'block' : 'none' }}
            onScroll={(e) => e.stopPropagation()}>
            <div className={`${cl.model_review_window} ${modalWindowDimensions.height > window.innerHeight ? cl.model_upload_window_fixed : ''}`} ref={modalWindowRef}>
                <img className={cl.model_review_window_cancel_sign}
                    alt="cancel"
                    onClick={onClose} />
            </div>
        </div>
    );
}

export default ReviewModelWindow;
import React from 'react';
import UploadModelWindow from './components/UploadModelWindow/UploadModelWindow';
import ReviewModelWindow from './components/ReviewModelWindow/ReviewModelWindow';

export const UploadModelWindowContext = React.createContext(null);
export const ReviewModelWindowContext = React.createContext(null);

export const ContextProvider = ({ children }) => {
    const [uploadModelWindowInfo, setUploadModelWindowInfo] = React.useState({
        visible: false,
        modelId: null
    });
    const [uploadModelWindowEvents, setUploadModelWindowEvents] = React.useState({
        onUpload: () => { },
        onClose: () => { }
    });

    const [reviewModelWindowInfo, setReviewModelWindowInfo] = React.useState({
        visible: false,
        modelId: null
    });
    const [reviewModelWindowEvents, setReviewModelWindowEvents] = React.useState({
        onClose: () => { },
        onAccept: () => { },
        onDeny: () => { },
        onBlock: () => { }
    });

    return (
        <UploadModelWindowContext.Provider value={{
            uploadModelWindowInfo,
            uploadModelWindowEvents,
            setUploadModelWindowInfo,
            setUploadModelWindowEvents
        }}>
            <ReviewModelWindowContext.Provider value={{
                reviewModelWindowInfo,
                reviewModelWindowEvents,
                setReviewModelWindowInfo,
                setReviewModelWindowEvents
            }}>
                {children}
                <UploadModelWindow
                    visible={uploadModelWindowInfo.visible}
                    editingModelId={uploadModelWindowInfo.modelId}
                    onUpload={() => uploadModelWindowEvents.onUpload()}
                    onClose={() => uploadModelWindowEvents.onClose()} />
                <ReviewModelWindow
                    visible={reviewModelWindowInfo.visible}
                    reviewingModelId={reviewModelWindowInfo.modelId}
                    onClose={() => reviewModelWindowEvents.onClose()}
                    onAccept={() => reviewModelWindowEvents.onAccept()}
                    onDeny={() => reviewModelWindowEvents.onDeny()}
                    onBlock={() => reviewModelWindowEvents.onBlock()} />
            </ReviewModelWindowContext.Provider>
        </UploadModelWindowContext.Provider>
    );
}
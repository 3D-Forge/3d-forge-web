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
    const [reviewModelWindowInfo, setReviewModelWindowInfo] = React.useState({
        visible: false,
        modelId: null
    });

    return (
        <UploadModelWindowContext.Provider value={{ uploadModelWindowInfo, setUploadModelWindowInfo }}>
            <ReviewModelWindowContext.Provider value={{ reviewModelWindowInfo, setReviewModelWindowInfo }}>
                {children}
                <UploadModelWindow
                    visible={uploadModelWindowInfo.visible}
                    editingModelId={uploadModelWindowInfo.modelId}
                    onUpload={() => {
                        setUploadModelWindowInfo({ visible: false, modelId: null });
                        alert(uploadModelWindowInfo.modelId ? 'Model info is updated!' : 'Your model is uploaded!');
                    }}
                    onClose={() => setUploadModelWindowInfo({ visible: false, modelId: null })} />
                <ReviewModelWindow
                    visible={reviewModelWindowInfo.visible}
                    reviewingModelId={reviewModelWindowInfo.modelId}
                    onClose={() => setReviewModelWindowInfo({ visible: false, modelId: null })} />
            </ReviewModelWindowContext.Provider>
        </UploadModelWindowContext.Provider>
    );
}
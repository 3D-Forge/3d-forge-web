import cl from './.module.css';

const LoadingAnimation = ({ size = '120px', loadingCurveWidth = '16px', isCenter = false }) => {

    const centeredStyle = { display: 'flex', alignItems: 'center', justifyContent: 'center' };

    return (
        <div style={isCenter ? centeredStyle : { display: 'block' }}>
            <div className={cl.loader} style={{
            width: size,
            height: size,
            border: `${loadingCurveWidth} solid #00a193`,
            borderTop: `${loadingCurveWidth} solid #C99E22`,
            borderBottom: `${loadingCurveWidth} solid #C99E22`
            }}></div>
        </div>
    );
}

export default LoadingAnimation;
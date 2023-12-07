import React from "react";
import { CatalogAPI } from "../../services/api/CatalogAPI";
import cl from "./.module.css";
import { CartAPI } from "../../services/api/CartAPI";
import { CatologAPI } from "../../services/api/CatalogAPI";
const CartPage = () => {
    const [modelsInfo, setModelsInfo] = React.useState(undefined);
    const [quantities, setQuantities] = React.useState({});
    const [selectedModels, setSelectedModels] = React.useState([]);

    React.useEffect(() => {
        let isMounted = true;
        const fetchData = async () => {
            try {
                const response = await CartAPI.getItems();
                console.log(response.status);
                if (response.ok) {
                    const resModel = await response.json();
                    console.log(resModel.data);
                    let Models = [];
                    for (let i = 0; i < resModel.data.orderedModelIDs.length; i++) {
                        const modelResponse = await CatalogAPI.getModel(
                            resModel.data.orderedModelIDs[i].id);
                        Models[i] = await modelResponse.json();
                        //console.log(modelResponse.json())
                    }
                    console.log(Models);
                    setModelsInfo(Models);
                    console.log(modelsInfo);

                    // Initialize quantities state for each model
                    const initialQuantities = resModel.data.orderedModelIDs.reduce((acc, model) => {
                        acc[model.id] = 1;
                        return acc;
                    }, {});
                    setQuantities(initialQuantities);
                }
                else {
                    console.error('Помилка отримання моделі:', response.statusText);
                }
            } catch (error) {
                console.error('Помилка отримання моделі:', error);
            }
        };

        fetchData();

        return () => {
            isMounted = false;
        };
    }, []);

    const handleInputChange = (event, modelId) => {
        const newValue = parseInt(event.target.value, 10) || 0;
        setQuantities((prevQuantities) => ({
            ...prevQuantities,
            [modelId]: newValue,
        }));
    };

    const handleArrowClick = (direction, modelId) => {
        setQuantities((prevQuantities) => ({
            ...prevQuantities,
            [modelId]: direction === 'up' ? prevQuantities[modelId] + 1 : Math.max(prevQuantities[modelId] - 1, 1),
        }));
    };
    const handleCheckboxChange = (modelId) => {
        setSelectedModels((prevSelected) => {
            if (prevSelected.includes(modelId)) {
                return prevSelected.filter((id) => id !== modelId);
            } else {
                return [...prevSelected, modelId];
            }
        });
    };
    const calculateTotalPrice = () => {
        let sum = 0;
        modelsInfo?.map((model) => {
            sum += model.data.depth * quantities[model.data.id];
        });
        return sum;
    };
    const info = (model) => {
        console.log(model);
    }
    function RenderCatalogSection() {
        if (Array.isArray(modelsInfo)) {
            return (
                <div className={cl.page}>
                    <div className={cl.models_group}>
                        <p className={cl.Position}> Позиція </p>
                        <p className={cl.Depth}>Ціна</p>
                        <p className={cl.Count}>Кількість</p>
                        <p className={cl.Sum}>Загалом</p></div>
                    {modelsInfo?.map((model) => (
                        
                        <div key={model.data.id} className={cl.model_item}>
                            {model && model.data.picturesIDs && model.data.picturesIDs.length > 0 && (
                                <img className={cl.model_image} src={`/api/catalog/model/picture/${model.data.picturesIDs[0]}`} alt={`Model ${model.name}`} />
                            )}
                            <input
                                className={cl.checkbox}
                                type="checkbox"
                                name="verification"
                                checked={selectedModels.includes(model.data.id)}
                                onChange={() => handleCheckboxChange(model.data.id)}
                            />
                            <p className={cl.model_Name}> {model.data.name}</p>
                            <p className={cl.model_Depth}> {model.data.depth}₴</p>
                            <input
                                className={cl.count_input}
                                type="number"
                                id={`quantity-${model.data.id}`}
                                name={`quantity-${model.data.id}`}
                                defaultValue={1}
                              
                                min="1"
                                onChange={(event) => handleInputChange(event, model.data.id)}
                            />
                            <p className={cl.model_Sum}> {model.data.depth * quantities[model.data.id]}₴</p>
                        </div>
                    ))}
                </div>
            );
        } else {
            // Handle the case where modelsInfo is not an array
            return <p>modelsInfo is not an array.</p>;
        }
    }

    return (
        <div>
            <div>{RenderCatalogSection()}</div>
            <button className={cl.back_button}>{'<'}Повернутися до каталогу</button>
            <div className={cl.order_group}>
                <div>За моделі: {calculateTotalPrice()}₴</div>
                <div>За доставку: 150₴</div>
                <br></br>
                <div>Загалом: {calculateTotalPrice() + 150}₴</div>
                <br></br>
                <br></br>
                <div className={cl.buy_model_button}>
                    <img className={cl.buy_model_button_img} alt="buy model" />
                    <span className={cl.buy_model_button_text}>Придбати</span>
                </div>
                <br></br>
            </div>
        </div>
    );
};

export default CartPage;

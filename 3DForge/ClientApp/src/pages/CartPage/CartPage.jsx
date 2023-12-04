import React from "react";
import { CatalogAPI } from "../../services/api/CatalogAPI";
import cl from "./.module.css";
const CartPage = () => {
    const [modelsInfo, setModelsInfo] = React.useState(undefined);
    const [quantities, setQuantities] = React.useState({});

    React.useEffect(() => {
        let isMounted = true;
        const fetchData = async () => {
            try {
                const response = await CatalogAPI.GetUnacceptedModels();
                console.log(response.status);
                if (response.ok) {
                    const resModel = await response.json();
                    console.log(resModel);
                    setModelsInfo(resModel.data);

                    // Initialize quantities state for each model
                    const initialQuantities = resModel.data.reduce((acc, model) => {
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

    function RenderCatalogSection() {
        if (Array.isArray(modelsInfo)) {
            return (
                <div className={cl.page}>
                    <div className={cl.models_group}>
                        <p className={cl.Position}> Позиція </p>
                        <p className={cl.Depth}>Ціна</p>
                        <p className={cl.Count}>Кількість</p>
                        <p className={cl.Sum}>Загалом</p></div>
                    {modelsInfo.map((model) => (
                        <div key={model.id} className={cl.model_item}>
                            <input className={cl.checkbox} type="checkbox" name="verification"></input>
                            <img className={cl.model_image} src={`/api/catalog/model/picture/${model.picturesIDs[0]}`}></img>
                            <p className={cl.model_Name}> {model.name}</p>
                            <p className={cl.model_Depth}> {model.depth}₴</p>
                            <input className={cl.count_input}
                                type="number"
                                id={`quantity-${model.id}`}
                                name={`quantity-${model.id}`}
                                value={quantities[model.id]}
                                min="1"
                                onChange={(event) => handleInputChange(event, model.id)}
                            />
                            <p className={cl.model_Sum}> {model.depth * quantities[model.id]}₴</p>
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
            <button>Повернутися до каталогу</button>
            <div class={cl.order_group}>
                <div>За моделі</div>
                <div>За доставку</div>
                <div>Загалом</div>
                <button>Замовити</button>
            </div>
        </div>
    );
};

export default CartPage;

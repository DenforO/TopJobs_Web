import React, { useCallback, useState, useEffect } from "react";
import Select from 'react-select';
import makeAnimated from 'react-select/animated';
import {
    LineChart,
    Line,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    Legend
} from "recharts";

const animatedComponents = makeAnimated();

const technologyArray = [
    { value: 8, label: 'CSS' },
    { value: 9, label: 'React.js' },
    { value: 10, label: 'Vue.js' }
]

const dataJson = [
    {
        name: "Page A",
        uv: 4000,
        pv: 2400,
        amt: 2400
    },
    {
        name: "Page B",
        uv: 3000,
        pv: 1398,
        amt: 2210
    },
    {
        name: "Page C",
        uv: 2000,
        pv: 9800,
        amt: 2290
    },
    {
        name: "Page D",
        uv: 2780,
        pv: 3908,
        amt: 2000
    },
    {
        name: "Page E",
        uv: 1890,
        pv: 4800,
        amt: 2181
    },
    {
        name: "Page F",
        uv: 2390,
        pv: 3800,
        amt: 2500
    },
    {
        name: "Page G",
        uv: 3490,
        pv: 4300,
        amt: 2100
    }
];

const dataJson2 = [
    {
        "month": "May 20",
        "C#": 0,
        "JavaScript": 5,
        "SQL": 6
    },
    {
        "month": "Jun 20",
        "C#": 10,
        "JavaScript": 4,
        "SQL": 3
    },
    {
        "month": "Jul 20",
        "C#": 5,
        "JavaScript": 5,
        "SQL": 7
    },
    {
        "month": "Aug 20",
        "C#": 9,
        "JavaScript": 1,
        "SQL": 6
    },
    {
        "month": "Sep 20",
        "C#": 4,
        "JavaScript": 3,
        "SQL": 4
    }
];

const getRandomColor = () => {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

const getTechnologyColor = (name) => {
    var letters = '0123456789ABCDEF';
    var color = '#';
    while (name.length < 6) {
        name += name;
    }
    for (var i = 0; i < 6; i++) {
        color += letters[(name.charCodeAt(i) + ((i + 1) * 2)) % 16];
    }
    return color;
}

const colors = ['#4287f5', '#15d11e', '#e03b16', '#f431f7', '#f2de02', '#288a15', '#5a0c8a', '#2af3fa', '#8a1e08', '#031c80']

function Button(props) {
    return (
        <button onClick={props.onClickFunction}>Add</button>
    )
}

function Input(props) {
    const [inputValue, setInputValue] = useState(['8', 'CSS']);

    return (
        <div>
            <input type="text" onKeyUp={(e) => setInputValue(e.target.value.split(','))} />
            <Button onClickFunction={() => props.addTechnology(inputValue)} />
        </div>
    )
}

function AutocompleteInput(props) {
    const [allTechnologies, setAllTechnologies] = useState([]);
    const [selectedTechnologies, setSelectedTechnologies] = useState([]);
    const fetchData = () => {
        fetch(window.location.origin + "/api/Trends/technologies")
            .then(response => {
                return response.json()
            })
            .then(data => {
                setAllTechnologies(data)
            })
    }
    useEffect(() => {
        fetchData()
    }, [])
    if (allTechnologies.length > 0) {
        console.log(allTechnologies);
        return (
            <Select
                closeMenuOnSelect={true}
                components={animatedComponents}
                defaultValue={[allTechnologies[0], allTechnologies[1]]}
                isMulti
                options={allTechnologies}
                isOptionDisabled={(choice) => selectedTechnologies >= 8}
                onChange={(choice) => { setSelectedTechnologies(choice); props.addTechnology(choice) }}
            />
        )
    }
    else {
        return null;
    }
}

export default function TrendChart() {
    const [data, setData] = useState();
    const [technologies, setTechnologies] = useState([{ value: '1', label: 'C#' }, { value: '2', label: 'C++' }]);

    const addTechnology = (tech) => {
        console.log('tech');
        console.log(tech);
        setTechnologies(
            tech
        );
    }

    const fetchData = () => {
        let queryParameters = "";
        console.log("technologies in fetch data")
        console.log(technologies)
        technologies.forEach(technology => queryParameters += ("techId=" + technology['value'] + "&"));
        console.log(queryParameters)
        fetch(window.location.origin + "/api/Trends/TechnologyTrend?" + queryParameters)
            .then(response => {
                return response.json()
            })
            .then(data => {
                setData(data)
            })
    }

    useEffect(() => {
        fetchData()
    }, [technologies])

    if (data != null) {
        return (
            <div>
                <LineChart
                    id="test"
                    width={1000}
                    height={300}
                    data={data}
                    margin={{
                        top: 15,
                        right: 30,
                        left: 20,
                        bottom: 5
                    }}
                >
                    <CartesianGrid strokeDasharray="3 3" />
                    <XAxis dataKey="Month" />
                    <YAxis />
                    <Tooltip />
                    <Legend />
                    {
                        technologies.map((technology) => {
                            return (<Line stroke={getTechnologyColor(technology['label'])} type="monotone" key={`line_${technology['label']}`} dataKey={`${technology['label']}`} />)
                        })
                    }
                </LineChart>
                <AutocompleteInput addTechnology={addTechnology}/>
            </div>
        );
    }
    console.log('returned null')
    return null; //if line chart gets rendered before data is fetched, animation gets broken
}

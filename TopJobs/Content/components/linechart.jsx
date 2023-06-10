import React, { useCallback, useState, useEffect } from "react";
import {
    LineChart,
    Line,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    Legend
} from "recharts";

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

export default function TrendChart() {
    const [data, setData] = useState(dataJson2);
    const [technologies, setTechnologies] = useState(['ads', 'uv']);

    //const fetchData = () => {
    //    fetch(window.location.origin + "/api/Trends/TechnologyTrend")
    //        .then(response => {
    //            return response.json()
    //        })
    //        .then(data => {
    //            setData(data)
    //        })
    //}
    //useEffect(() => {
    //    fetchData()
    //}, [])

    if (data != null) {
        return (
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
                <XAxis dataKey="month" />
                <YAxis />
                <Tooltip />
                <Legend />
                {
                    technologies.map((id) => {
                        return (<Line key={`line_${id}`} dataKey={`${id}`} />)
                    })
                }
                {/*<Line*/}
                {/*    type="monotone"*/}
                {/*    dataKey={technologies[0]}*/}
                {/*    stroke="#8884d8"*/}
                {/*    activeDot={{ r: 8 }}*/}
                {/*    animationDuration={5000}*/}
                {/*/>*/}
                {/*<Line type="monotone" dataKey={technologies[1]} stroke="#82ca9d" />*/}
            </LineChart>
        );
    }
    return null; //if line chart gets rendered before data is fetched, animation gets broken
}

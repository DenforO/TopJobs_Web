﻿import React, { useCallback, useState, useEffect } from "react";
import { PieChart, Pie, Sector } from "recharts";

const data = [
    { name: "JavaScript", value: 192 },
    { name: "Java", value: 145 },
    { name: "C#", value: 103 },
    { name: "Python", value: 102 }
];

const dataString = '[{  "name": "JavaScript", "value": 192 },{ "name": "Java", "value": 155 },{ "name": "C#", "value": 103 },{ "name": "Python", "value": 100 }]'

const renderActiveShape = (props) => {
    const RADIAN = Math.PI / 180;
    const {
        cx,
        cy,
        midAngle,
        innerRadius,
        outerRadius,
        startAngle,
        endAngle,
        fill,
        payload,
        percent,
        value
    } = props;
    const sin = Math.sin(-RADIAN * midAngle);
    const cos = Math.cos(-RADIAN * midAngle);
    const sx = cx + (outerRadius + 10) * cos;
    const sy = cy + (outerRadius + 10) * sin;
    const mx = cx + (outerRadius + 30) * cos;
    const my = cy + (outerRadius + 30) * sin;
    const ex = mx + (cos >= 0 ? 1 : -1) * 22;
    const ey = my;
    const textAnchor = cos >= 0 ? "start" : "end";

    return (
        <g>
            <text x={cx} y={cy} dy={8} textAnchor="middle" fill={fill}>
                {payload.name}
            </text>
            <Sector
                cx={cx}
                cy={cy}
                innerRadius={innerRadius}
                outerRadius={outerRadius}
                startAngle={startAngle}
                endAngle={endAngle}
                fill={fill}
            />
            <Sector
                cx={cx}
                cy={cy}
                startAngle={startAngle}
                endAngle={endAngle}
                innerRadius={outerRadius + 6}
                outerRadius={outerRadius + 10}
                fill={fill}
            />
            <path
                d={`M${sx},${sy}L${mx},${my}L${ex},${ey}`}
                stroke={fill}
                fill="none"
            />
            <circle cx={ex} cy={ey} r={2} fill={fill} stroke="none" />
            <text
                x={ex + (cos >= 0 ? 1 : -1) * 12}
                y={ey}
                textAnchor={textAnchor}
                fill="#333"
            >{`${value}`}</text>
            <text
                x={ex + (cos >= 0 ? 1 : -1) * 12}
                y={ey}
                dy={18}
                textAnchor={textAnchor}
                fill="#999"
            >
                {`(${(percent * 100).toFixed(2)}%)`}
            </text>
        </g>
    );
};

export default function PieChartDemo(props) {
    const [activeIndex, setActiveIndex] = useState(0);
    const [data, setData] = useState(null);

    const fetchData = () => {
        fetch("https://localhost:44363/api/Trends")
            .then(response => {
                return response.json()
            })
            .then(data => {
                setData(data)
            })
    }
    useEffect(() => {
        fetchData()
    }, [])

    const onPieEnter = useCallback(
        (_, index) => {
            setActiveIndex(index);
        },
        [setActiveIndex]
    );

    return (
        <PieChart width={600} height={400}>
            <Pie
                activeIndex={activeIndex}
                activeShape={renderActiveShape}
                data={data}
                cx={300}
                cy={200}
                innerRadius={100}
                outerRadius={130}
                fill="#8884d8"
                dataKey="value"
                onMouseEnter={onPieEnter}
            />
        </PieChart>
    );
}

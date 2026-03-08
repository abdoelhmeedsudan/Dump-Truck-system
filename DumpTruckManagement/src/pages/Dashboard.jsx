import React, { useState, useEffect } from 'react';
import {
    BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer,
    PieChart, Pie, Cell, AreaChart, Area
} from 'recharts';
import {
    Truck, Users, DollarSign, TrendingUp,
    Activity, AlertCircle, CheckCircle, Clock
} from 'lucide-react';
import { dashboardApi, extractObjectFromResponse } from '../services/apiService';
import './Dashboard.css';

const Dashboard = () => {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [data, setData] = useState(null);

    useEffect(() => {
        loadDashboardData();
    }, []);

    async function loadDashboardData() {
        try {
            setLoading(true);
            const response = await dashboardApi.getStats();
            setData(extractObjectFromResponse(response));
        } catch (err) {
            console.error("Error loading dashboard data:", err);
            setError("حدث خطأ أثناء تحميل البيانات");
        } finally {
            setLoading(false);
        }
    }

    if (loading) {
        return (
            <div className="dashboard-container" style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '400px' }}>
                <div className="loading"></div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="dashboard-container">
                <div className="badge badge-error" style={{ padding: '1rem' }}>{error}</div>
            </div>
        );
    }

    if (!data) return null;

    // Map API data to UI components
    const stats = [
        { title: 'إجمالي الشاحنات', value: data.stats?.totalTrucks || 0, icon: Truck, color: 'text-blue', bg: 'bg-blue' },
        { title: 'السائقين النشطين', value: data.stats?.activeDrivers || 0, icon: Users, color: 'text-green', bg: 'bg-green' },
        {
            title: 'الإيرادات الشهرية',
            value: new Intl.NumberFormat('ar-EG-u-nu-latn', { style: 'currency', currency: 'SDG' }).format(data.stats?.monthlyRevenue || 0),
            icon: DollarSign,
            color: 'text-indigo',
            bg: 'bg-indigo'
        },
        {
            title: 'صافي الربح',
            value: new Intl.NumberFormat('ar-EG-u-nu-latn', { style: 'currency', currency: 'SDG' }).format(data.stats?.netProfit || 0),
            icon: TrendingUp,
            color: 'text-purple',
            bg: 'bg-purple'
        },
    ];

    const revenueData = (data.revenueExpenseChart || []).map(item => ({
        name: item.month,
        revenue: item.revenue,
        expenses: item.expenses
    }));

    const truckStatusData = [
        { name: 'نشط', value: data.fleetStatus?.activeTrucks || 0, color: '#10b981' },
        { name: 'صيانة', value: data.fleetStatus?.maintenanceTrucks || 0, color: '#f59e0b' },
        { name: 'متوقف', value: data.fleetStatus?.inactiveTrucks || 0, color: '#64748b' },
    ].filter(item => item.value > 0);

    const getActivityIcon = (type) => {
        switch (type) {
            case 'MaintenanceComplete': return { icon: CheckCircle, color: 'text-green' };
            case 'PaymentReceipt': return { icon: DollarSign, color: 'text-indigo' };
            case 'Alert': return { icon: AlertCircle, color: 'text-red' };
            case 'Shift': return { icon: Clock, color: 'text-blue' };
            default: return { icon: Activity, color: 'text-gray' };
        }
    };

    const formatTimeAgo = (dateString) => {
        const date = new Date(dateString);
        const now = new Date();
        const diffInSeconds = Math.floor((now - date) / 1000);

        if (diffInSeconds < 60) return 'منذ لحظات';
        if (diffInSeconds < 3600) return `منذ ${Math.floor(diffInSeconds / 60)} دقيقة`;
        if (diffInSeconds < 86400) return `منذ ${Math.floor(diffInSeconds / 3600)} ساعة`;
        return `منذ ${Math.floor(diffInSeconds / 86400)} يوم`;
    };

    const recentActivity = (data.recentActivities || []).map((activity, index) => {
        const style = getActivityIcon(activity.activityType);
        return {
            id: index,
            desc: activity.description, // using description as main text
            sub: activity.title,
            time: formatTimeAgo(activity.activityDate),
            icon: style.icon,
            color: style.color
        };
    });

    return (
        <div className="dashboard-container">
            <div className="dashboard-header">
                <div>
                    <h1 className="dashboard-title">نظرة عامة</h1>
                    <p className="dashboard-subtitle">مرحباً بك، إليك ملخص لما يحدث اليوم.</p>
                </div>
                <div className="dashboard-actions">
                    <button className="btn-secondary" onClick={loadDashboardData}>
                        تحديث البيانات
                    </button>
                    <button className="btn-primary" onClick={() => window.print()}>
                        تحميل التقرير
                    </button>
                </div>
            </div>

            {/* Stats Grid */}
            <div className="stats-grid">
                {stats.map((stat, index) => (
                    <div key={index} className="stat-card">
                        <div className="stat-header">
                            <div>
                                <p className="stat-title">{stat.title}</p>
                                <h3 className="stat-value">{stat.value}</h3>
                            </div>
                            <div className={`stat-icon-wrapper ${stat.bg}`}>
                                <stat.icon className={`w-6 h-6 ${stat.color}`} size={24} />
                            </div>
                        </div>
                        {/* Trend logic could be added if API supports it */}
                    </div>
                ))}
            </div>

            {/* Charts Section */}
            <div className="charts-grid">
                {/* Revenue Chart */}
                <div className="chart-card">
                    <h3 className="chart-title">الإيرادات والمصروفات</h3>
                    <div className="chart-container">
                        <ResponsiveContainer width="100%" height="100%">
                            <AreaChart data={revenueData} margin={{ top: 10, right: 0, left: 30, bottom: 0 }}>
                                <defs>
                                    <linearGradient id="colorRevenue" x1="0" y1="0" x2="0" y2="1">
                                        <stop offset="5%" stopColor="#4f46e5" stopOpacity={0.1} />
                                        <stop offset="95%" stopColor="#4f46e5" stopOpacity={0} />
                                    </linearGradient>
                                    <linearGradient id="colorExpenses" x1="0" y1="0" x2="0" y2="1">
                                        <stop offset="5%" stopColor="#ef4444" stopOpacity={0.1} />
                                        <stop offset="95%" stopColor="#ef4444" stopOpacity={0} />
                                    </linearGradient>
                                </defs>
                                <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#e2e8f0" />
                                <XAxis dataKey="name" axisLine={false} tickLine={false} tick={{ fill: '#64748b' }} dy={10} />
                                <YAxis axisLine={false} tickLine={false} tick={{ fill: '#64748b' }} orientation="right" />
                                <Tooltip
                                    contentStyle={{ backgroundColor: '#fff', borderRadius: '8px', border: '1px solid #e2e8f0', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)', textAlign: 'right' }}
                                    itemStyle={{ color: '#1e293b' }}
                                    formatter={(value) => new Intl.NumberFormat('en-US').format(value)}
                                />
                                <Area type="monotone" dataKey="revenue" stroke="#4f46e5" strokeWidth={2} fillOpacity={1} fill="url(#colorRevenue)" name="الإيرادات" />
                                <Area type="monotone" dataKey="expenses" stroke="#ef4444" strokeWidth={2} fillOpacity={1} fill="url(#colorExpenses)" name="المصروفات" />
                            </AreaChart>
                        </ResponsiveContainer>
                    </div>
                </div>

                {/* Truck Status & Activity */}
                <div className="flex flex-col gap-6" style={{ display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
                    {/* Pie Chart */}
                    <div className="chart-card">
                        <h3 className="chart-title">حالة الأسطول</h3>
                        <div className="pie-chart-container">
                            <ResponsiveContainer width="100%" height="100%">
                                <PieChart>
                                    <Pie
                                        data={truckStatusData}
                                        cx="50%"
                                        cy="50%"
                                        innerRadius={60}
                                        outerRadius={80}
                                        paddingAngle={5}
                                        dataKey="value"
                                    >
                                        {truckStatusData.map((entry, index) => (
                                            <Cell key={`cell-${index}`} fill={entry.color} />
                                        ))}
                                    </Pie>
                                    <Tooltip />
                                </PieChart>
                            </ResponsiveContainer>
                        </div>
                        <div className="pie-legend">
                            {truckStatusData.map((item, index) => (
                                <div key={index} className="legend-item">
                                    <div className="legend-color" style={{ backgroundColor: item.color }}></div>
                                    <span>{item.name} ({item.value})</span>
                                </div>
                            ))}
                        </div>
                    </div>

                    {/* Recent Activity */}
                    <div className="chart-card" style={{ flex: 1 }}>
                        <h3 className="chart-title">النشاط الأخير</h3>
                        <div className="activity-list">
                            {recentActivity.length === 0 ? (
                                <div style={{ textAlign: 'center', color: 'var(--muted)', padding: '1rem' }}>لا يوجد نشاط مؤخرًا</div>
                            ) : (
                                recentActivity.map((activity) => (
                                    <div key={activity.id} className="activity-item">
                                        <div className={`activity-icon ${activity.color}`}>
                                            <activity.icon size={20} />
                                        </div>
                                        <div className="activity-content">
                                            <p className="activity-desc" title={activity.desc}>{activity.sub} - <span style={{ fontSize: '0.85em', color: 'var(--muted)' }}>{activity.desc}</span></p>
                                            <p className="activity-time">{activity.time}</p>
                                        </div>
                                    </div>
                                ))
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Dashboard;

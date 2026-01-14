import React from 'react';
import {
    BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer,
    PieChart, Pie, Cell, AreaChart, Area
} from 'recharts';
import {
    Truck, Users, DollarSign, TrendingUp,
    Activity, AlertCircle, CheckCircle, Clock
} from 'lucide-react';
import './Dashboard.css';

const Dashboard = () => {
    // Mock Data
    const stats = [
        { title: 'إجمالي الشاحنات', value: '24', icon: Truck, color: 'text-blue', bg: 'bg-blue' },
        { title: 'السائقين النشطين', value: '18', icon: Users, color: 'text-green', bg: 'bg-green' },
        { title: 'الإيرادات الشهرية', value: '124,500 ج.م', icon: DollarSign, color: 'text-indigo', bg: 'bg-indigo' },
        { title: 'صافي الربح', value: '42,300 ج.م', icon: TrendingUp, color: 'text-purple', bg: 'bg-purple' },
    ];

    const revenueData = [
        { name: 'يناير', revenue: 65000, expenses: 40000 },
        { name: 'فبراير', revenue: 72000, expenses: 45000 },
        { name: 'مارس', revenue: 85000, expenses: 48000 },
        { name: 'أبريل', revenue: 92000, expenses: 51000 },
        { name: 'مايو', revenue: 88000, expenses: 53000 },
        { name: 'يونيو', revenue: 124500, expenses: 62000 },
    ];

    const truckStatusData = [
        { name: 'نشط', value: 18, color: '#10b981' },
        { name: 'صيانة', value: 4, color: '#f59e0b' },
        { name: 'متوقف', value: 2, color: '#64748b' },
    ];

    const recentActivity = [
        { id: 1, type: 'Shift', desc: 'السائق أحمد بدأ الوردية #1023', time: 'منذ ساعتين', icon: Clock, color: 'text-blue' },
        { id: 2, type: 'Maintenance', desc: 'تم الانتهاء من صيانة الشاحنة DT-04', time: 'منذ 4 ساعات', icon: CheckCircle, color: 'text-green' },
        { id: 3, type: 'Alert', desc: 'الشاحنة DT-09 أبلغت عن مشكلة في المحرك', time: 'منذ 5 ساعات', icon: AlertCircle, color: 'text-red' },
        { id: 4, type: 'Revenue', desc: 'تم استلام دفعة للموقع ب', time: 'منذ يوم واحد', icon: DollarSign, color: 'text-indigo' },
    ];

    return (
        <div className="dashboard-container">
            <div className="dashboard-header">
                <div>
                    <h1 className="dashboard-title">نظرة عامة</h1>
                    <p className="dashboard-subtitle">مرحباً بك، إليك ملخص لما يحدث اليوم.</p>
                </div>
                <div className="dashboard-actions">
                    <button className="btn-secondary">
                        تحميل التقرير
                    </button>
                    <button className="btn-primary">
                        إضافة سجل جديد
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
                        <div className="stat-trend">
                            <span className="trend-up">
                                <TrendingUp size={16} className="ml-1" /> +12.5%
                            </span>
                            <span className="trend-label">عن الشهر الماضي</span>
                        </div>
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
                                    {item.name}
                                </div>
                            ))}
                        </div>
                    </div>

                    {/* Recent Activity */}
                    <div className="chart-card" style={{ flex: 1 }}>
                        <h3 className="chart-title">النشاط الأخير</h3>
                        <div className="activity-list">
                            {recentActivity.map((activity) => (
                                <div key={activity.id} className="activity-item">
                                    <div className={`activity-icon ${activity.color}`}>
                                        <activity.icon size={20} />
                                    </div>
                                    <div className="activity-content">
                                        <p className="activity-desc">{activity.desc}</p>
                                        <p className="activity-time">{activity.time}</p>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Dashboard;
